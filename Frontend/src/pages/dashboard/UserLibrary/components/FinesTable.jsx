import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Button } from "@material-tailwind/react";
import { toast } from 'react-toastify';
import { useUser } from '../../../../context/LoginRequired';
import { WalletIcon } from '@heroicons/react/24/solid';
const FinesTable = () => {
  const [fines, setFines] = useState([]);
  const user = useUser();
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(10);

  const pageCount = Math.ceil(fines.length / itemsPerPage);

  const changePage = (newPage) => {
    setCurrentPage(newPage);
  };

  const currentFines = fines.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  useEffect(() => {
    const fetchFines = async () => {
      try {
        const response = await axios.get(`https://localhost:7115/api/v1/fines/getfinesbyuser/${user.userId}`);
        const finesWithBookDetails = await Promise.all(response.data.map(async fine => {
          const bookDetails = await axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${fine.bookId}`);
          return { ...fine, bookDetails: bookDetails.data };
        }));
        setFines(finesWithBookDetails);
      } catch (error) {
        console.error('Failed to fetch fines:', error);
        toast.error("Failed to fetch fines");
      }
    };
    fetchFines();
  }, [user.userId]);

  const handlePayFine = async (fineId) => {
    try {
      const { data } = await axios.post('https://localhost:7115/api/payment/pay-fine', {
        fineId: fineId  // Include ID-ul amenzii în corpul cererii
      });

      if (data.url) {
        window.location.href = data.url;  // Redirecționează utilizatorul către portalul de plată Stripe
      } else {
        toast.error("Link-ul de plată nu a fost generat.");
      }
    } catch (error) {
      console.error('Error fetching payment link:', error);
      toast.error("Eroare la obținerea link-ului de plată.");
    }
  };



  return (
    <div className="overflow-x-auto relative shadow-md sm:rounded-lg">
      <table className="w-full text-sm text-left text-gray-500 mt-5">
        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th scope="col" className="py-3 px-6">Overdue Book</th>
            <th scope="col" className="py-3 px-6">Return Date</th>
            <th scope="col" className="py-3 px-6">Days Overdue</th>
            <th scope="col" className="py-3 px-6">Amount</th>
            <th scope="col" className="py-3 px-6">Status</th>
            <th scope="col" className="py-3 px-6">Actions</th>
          </tr>
        </thead>
        <tbody>
          {currentFines.map((fine) => (
            <tr key={fine.fineId} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
              <td className="py-4 px-6 flex items-center space-x-3">
                <img src={fine.bookDetails.imageUrl || '/path/to/default/image.jpg'} alt={fine.bookDetails.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                <span>{fine.bookDetails.title}</span>
              </td>
              <td className="py-4 px-6">{new Date(fine.createdAt).toLocaleDateString()}</td>
              <td className="py-4 px-6">{fine.daysOverdue}</td>
              <td className="py-4 px-6">${fine.amount}</td>

              <td className="py-4 px-6">
                {fine.isPaid ?
                  <span className="text-green-500 font-bold">Paid</span> :
                  <span className="text-red-500 font-bold">Unpaid</span>
                }
              </td>
              <td className="py-4 px-6">
                {!fine.isPaid && (
                  <div className="group relative inline-block">
                    <button onClick={() => handlePayFine(fine.fineId)} className="text-green-500 hover:text-green-700">
                      <WalletIcon className="h-6 w-6" />
                    </button>
                    <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                      Pay fine
                    </span>
                  </div>
                )}

              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {pageCount > 1 && (  
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
  );
};

export default FinesTable;
