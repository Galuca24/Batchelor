import React, { useState, useEffect } from 'react';
import { Button } from "@material-tailwind/react";
import { Stack } from '@mui/material';
import axios from 'axios';
import { useUser } from '../../../../context/LoginRequired';
import { ReceiptRefundIcon } from '@heroicons/react/24/solid';
import { toast } from 'react-toastify';
import { sendNotification } from '../../../../services/notifications/sendNotification';

const LoansTable = () => {
  const [currentView, setCurrentView] = useState("AllLoans");
  const [loans, setLoans] = useState([]);
  const [activeLoans, setActiveLoans] = useState([]);
  const user = useUser();
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);


  function truncateString(str, num) {
    if (str.length > num) {
      return str.slice(0, num) + "...";
    } else {
      return str;
    }
  }

  const fetchLoansAndBooks = async () => {
    try {
      const loansResponse = await axios.get(`https://localhost:7115/api/v1/books/getuserloans/${user.userId}`);
      const loansData = loansResponse.data;

      const booksDetailsResponses = await Promise.all(
        loansData.map(loan =>
          axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${loan.bookId}`)
        )
      );

      const loansWithBookDetails = loansData.map((loan, index) => ({
        ...loan,
        bookDetails: booksDetailsResponses[index].data
      }));

      setLoans(loansWithBookDetails);
    } catch (error) {
      console.error('Failed to fetch loans or book details:', error);
    }
  };

  useEffect(() => {
    fetchLoansAndBooks();
  }, [user.userId]);

  const fetchActiveLoans = async () => {
    try {
      const { data } = await axios.get(`https://localhost:7115/api/v1/books/getuserloans/${user.userId}`);
      const activeLoansData = data.filter(loan => loan.returnDate === null);
      const booksDetailsResponses = await Promise.all(
        activeLoansData.map(loan =>
          axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${loan.bookId}`)
        )
      );
      const loansWithDetailsAndTime = await Promise.all(activeLoansData.map(async (loan, index) => {
        try {
          const timeResponse = await axios.get(`https://localhost:7115/api/v1/books/remaining-time/${loan.bookId}`);
          return {
            ...loan,
            bookDetails: booksDetailsResponses[index].data,
            remainingTime: timeResponse.data.remainingTime
          };
        } catch (error) {
          console.error('Error fetching remaining time:', error);
          return {
            ...loan,
            bookDetails: booksDetailsResponses[index].data,
            remainingTime: "N/A"
          };
        }
      }));

      setActiveLoans(loansWithDetailsAndTime);
    } catch (error) {
      console.error('Failed to fetch loans:', error);
      toast.error("Failed to fetch loans");
    }
  };

  useEffect(() => {
    fetchActiveLoans();
  }, [user.userId]);

  useEffect(() => {
    const intervalId = setInterval(() => {
      const updatedLoans = activeLoans.map(loan => {
        if (loan.remainingTime !== "N/A") {
          const dueTime = new Date(loan.dueDate);
          const now = new Date();
          let remainingTimeMs = dueTime.getTime() - now.getTime();
          let formattedTime = "Expired";

          if (remainingTimeMs > 0) {
            const days = Math.floor(remainingTimeMs / (24 * 60 * 60 * 1000));
            remainingTimeMs -= days * (24 * 60 * 60 * 1000);
            const hours = Math.floor(remainingTimeMs / (60 * 60 * 1000));
            remainingTimeMs -= hours * (60 * 60 * 1000);
            const minutes = Math.floor(remainingTimeMs / (60 * 1000));
            remainingTimeMs -= minutes * (60 * 1000);
            const seconds = Math.floor(remainingTimeMs / 1000);

            if (days > 0) {
              formattedTime = `${days} days`;
            } else if (hours > 0) {
              formattedTime = `${hours}h`;
            } else if (minutes > 0) {
              formattedTime = `${minutes}m ${seconds}s`;
            } else {
              formattedTime = `${seconds}s`;
            }
          }

          return { ...loan, remainingTime: formattedTime };
        }
        return loan;
      });
      setActiveLoans(updatedLoans);
    }, 1000);
    return () => clearInterval(intervalId);
  }, [activeLoans]);

  const handleReturnBook = async (bookId) => {
    try {
      const response = await axios.post('https://localhost:7115/api/v1/books/returnbook', {
        bookId: bookId,
        userId: user.userId
      });
      if (response.data.success) {
        toast.success("Book successfully returned");

        const favouritesResponse = await axios.get(`https://localhost:7115/api/v1/favourites/get-users-who-has-this-book-at-favourite/${bookId}`);
        const bookResponse = await axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${bookId}`);
        const bookTitle = bookResponse.data.title;
        
        console.log(bookTitle);
        
        if (favouritesResponse.data) {
          const favouriteUserIds = favouritesResponse.data;
          const currentUserUserId = user.userId;
          const adminResponse = await axios.get('https://localhost:7115/api/v1/users/admin');
          const adminUserId = adminResponse.data.users[0].userId;
      
          const otherUsersUserIds = favouriteUserIds.filter(userId => userId !== currentUserUserId && userId !== adminUserId);
          for (const userId of otherUsersUserIds) {
            await sendNotification(userId, `One of your favourite books ${bookTitle} has returned to the library!`);
          }
        } else {
          console.error('Favourites response is undefined:', favouritesResponse);
        }
      
        const adminResponse = await axios.get('https://localhost:7115/api/v1/users/admin');
        const adminUserId = adminResponse.data.users[0].userId;
        if (user.userId !== adminUserId) {

        await sendNotification(adminUserId, ` ${user.username} has returned book ${bookTitle}`);
        }

        await fetchLoansAndBooks();
        fetchActiveLoans();
      } else {
        toast.error(response.data.message);
      }
    } catch (error) {
      console.error('Error returning book:', error);
      toast.error("Failed to return book");
    }
  };

  

  const pageCount = Math.ceil(loans.length / itemsPerPage);

  // Schimbarea paginii curente
  const changePage = (newPage) => {
    setCurrentPage(newPage);
  };

  // Obținerea împrumuturilor pentru pagina curentă
  const currentLoans = loans.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );



  return (
    <div>
      <Stack direction="row" spacing={2} sx={{ mb: 2 }}>

        <Button
          onClick={() => setCurrentView('AllLoans')}
          className={`font-medium ${currentView === "AllLoans" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          All Loans
        </Button>
        <Button
          onClick={() => setCurrentView('ActiveLoans')}
          className={`font-medium ${currentView === "ActiveLoans" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          Active Loans
        </Button>
      </Stack>

      {currentView === 'AllLoans' && (
        <div>
          <table className="w-full  text-sm text-left text-gray-500 mt-5">
            <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
              <tr >
                <th scope="col" className="py-3 px-6">Book Title</th>
                <th scope="col" className="py-3 px-6">Loan Date</th>
                <th scope="col" className="py-3 px-6">Due Date</th>
                <th scope="col" className="py-3 px-6">Status</th>
              </tr>
            </thead>
            <tbody>
              {currentLoans.map((loan) => (
                <tr key={loan.loanId} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                  <td className="py-4 px-6 flex items-center space-x-3">
                    <img src={loan.bookDetails.imageUrl || '/path/to/default/image.jpg'} alt={loan.bookDetails.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                    <span>{truncateString(loan.bookDetails.title, 30)}</span>
                  </td>
                  <td className="py-4 px-6">{new Date(loan.loanDate).toLocaleDateString()}</td>
                  <td className="py-4 px-6">{new Date(loan.dueDate).toLocaleDateString()}</td>
                  <td className="py-4 px-6">
                    <div style={{ color: loan.returnDate ? 'green' : 'red', fontWeight: 'bold' }}>
                      {loan.returnDate ? 'Returned' : 'Loaned'}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          {pageCount > 1 && (  // Afișează butoanele de paginare doar dacă există mai mult de o pagină
            <div className="flex justify-center p-4 text-black">
              {[...Array(pageCount).keys()].map(n => (
                <button
                  key={n + 1}
                  onClick={() => changePage(n + 1)}
                  className={`px-3 py-1 ${currentPage === n + 1 ? 'text-white bg-surface-dark-green' : 'bg-white'}`}>
                  {n + 1}
                </button>
              ))}
            </div>
          )}
        </div>
      )}

      {currentView === 'ActiveLoans' && (
        <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-transparent">
          {activeLoans.length > 0 ? (
            <table className="w-full text-sm text-left text-gray-500">
              <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                <tr>
                  <th scope="col" className="py-3 px-6">Book</th>
                  <th scope="col" className="py-3 px-6">Loan Date</th>
                  <th scope="col" className="py-3 px-6">Due Date</th>
                  <th scope="col" className="py-3 px-6">Remaining Time</th>
                  <th scope="col" className="py-3 px-6">Actions</th>
                </tr>
              </thead>
              <tbody>
                {activeLoans.map((loan, index) => (
                  <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    <td className="py-4 px-6 flex items-center space-x-3">
                      <img src={loan.bookDetails.imageUrl || '/path/to/default/image.jpg'} alt={loan.bookDetails.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                      <span>{loan.bookDetails.title}</span>
                    </td>
                    <td className="py-4 px-6">{new Date(loan.loanDate).toLocaleDateString()}</td>
                    <td className="py-4 px-6">{new Date(loan.dueDate).toLocaleDateString()}</td>
                    <td className="py-4 px-6">{loan.remainingTime}</td>
                    <td className="py-4 px-6">
                      <div className="group relative inline-block">
                        <button className="text-red-500 hover:text-red-700" onClick={() => handleReturnBook(loan.bookId)}>
                          <ReceiptRefundIcon className="h-6 w-6" />
                        </button>
                        <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                          Return Book
                        </span>
                      </div>
                    </td>

                  </tr>
                ))}
              </tbody>
            </table>
          ) : (
            <div className="text-center py-10 bg-transparent !important">
              <img src="/img/sadBook.PNG" alt="No active loans" className="mx-auto bg-transparent xl:w-4/12 lg:w-6/12" />
              <p className="mt-2 text-gray-500">No active loans to display</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
};

export default LoansTable;
