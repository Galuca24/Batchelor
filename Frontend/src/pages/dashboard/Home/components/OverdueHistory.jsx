import React, { useEffect, useState } from "react";
import axios from "axios";
import { Avatar } from "@material-tailwind/react";

const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

const OverdueHistory = () => {
    const [overdueBooks, setOverdueBooks] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(4); // Setează numărul de cărți afișate pe pagină

    function truncateString(str, num) {
        if (str.length > num) {
          return str.slice(0, num) + "...";
        } else {
          return str;
        }
      }
    useEffect(() => {
        const fetchOverdueBooks = async () => {
            try {
                const response = await axios.get('https://localhost:7115/api/v1/Books/GetOverdueCheckouts');
                const booksWithPhotos = await Promise.all(response.data.map(async (book) => {
                    try {
                        const photoResponse = await axios.get(`https://localhost:7115/api/Cloud/get-user-photo?userId=${book.userId}`);
                        const fullPhotoUrl = s3BaseUrl + photoResponse.data.userPhotoUrl;
                        return {...book, userPhotoUrl: fullPhotoUrl};
                    } catch (photoError) {
                        console.error('Error fetching photo: ', photoError);
                        return {...book, userPhotoUrl: "/img/placeholder.png"};
                    }
                }));
                setOverdueBooks(booksWithPhotos);
            } catch (error) {
                console.error('Error fetching overdue books: ', error);
            }
        };

        fetchOverdueBooks();
    }, []);

    // Calculează numărul total de pagini
    const pageCount = Math.ceil(overdueBooks.length / itemsPerPage);

    // Schimbă pagina curentă
    const changePage = (newPage) => {
        setCurrentPage(newPage);
    };

    // Obține cărțile pentru pagina curentă
    const currentBooks = overdueBooks.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    return (
        <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white">
            <h2 className="text-lg font-semibold text-gray-700 p-4">Overdue Books</h2>
            <table className="w-full text-sm text-left text-gray-500">
                <thead className="text-xs text-gray-700 uppercase bg-gray-50">
                    <tr>
                        <th scope="col" className="py-3 px-6">User</th>
                        <th scope="col" className="py-3 px-6">Title</th>
                        <th scope="col" className="py-3 px-6">Author</th>
                        <th scope="col" className="py-3 px-6">Overdue Days</th>
                        <th scope="col" className="py-3 px-6">Fine</th>
                    </tr>
                </thead>
                <tbody>
                    {currentBooks.map((book, index) => (
                        <tr key={index} className="bg-white border-b">
                            <td className="py-4 px-6 flex items-center">
                                <Avatar src={book.userPhotoUrl || "/img/placeholder.png"} size="sm" />
                                <span className="ml-3">{book.userName}</span>
                            </td>
                            <td className="py-4 px-6">{truncateString(book.bookTitle,25)}</td>
                            <td className="py-4 px-6">{truncateString(book.author,15)}</td>
                            <td className="py-4 px-6">{book.daysOverdue}</td>
                            <td className="py-4 px-6">{book.fine}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
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
        </div>
    );
};

export default OverdueHistory;
