import React, { useState } from 'react';
import { PencilIcon, TrashIcon, EyeIcon } from "@heroicons/react/24/solid";
import DeleteBookConfirmationDialog from "./DeleteBookConfirmationDialog";
import EditBookModal from "./EditBookModal";
import BookDetailsModalForAdmin from "./BookDetailsModalForAdmin";
import { toast } from 'react-toastify';
import axios from 'axios';

const BooksTable = ({ books, onDelete, onUpdate, setBooks }) => {
    const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
    const [selectedBook, setSelectedBook] = useState(null);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [isDetailsModalOpen, setIsDetailsModalOpen] = useState(false);
    const [currentPage, setCurrentPage] = useState(1);
    const itemsPerPage = 10;

    const handleViewBook = (book) => {
        setSelectedBook(book);
        setIsDetailsModalOpen(true);
    };

    const handleEdit = (book) => {
        setSelectedBook(book);
        setIsEditModalOpen(true);
    };

    const handleBookUpdated = (updatedBook) => {
        const updatedBooks = books.map(book => book.bookId === updatedBook.bookId ? updatedBook : book);
        setBooks(updatedBooks);
        setIsEditModalOpen(false);
    };

    const handleOpenDeleteDialog = (book) => {
        setSelectedBook(book);
        setOpenDeleteDialog(true);
    };

    const handleCloseDeleteDialog = () => {
        setOpenDeleteDialog(false);
    };

    const handleConfirmDelete = async (bookId) => {
        try {
            const response = await axios.delete(`https://localhost:7115/api/v1/Books/${bookId}`);
            if (response.data.success) {
                toast.success(response.data.message || "Book has been deleted successfully.");
                onDelete(bookId);
            } else {
                toast.error("Failed to delete the book");
            }
        } catch (error) {
            console.error('Error deleting book:', error);
            toast.error("Error occurred while deleting the book");
        }
        setOpenDeleteDialog(false);
    };

    function truncateString(str, num) {
        if (str.length > num) {
            return str.slice(0, num) + "...";
        } else {
            return str;
        }
    }

    // Funcția pentru schimbarea paginii curente
    const changePage = (newPage) => {
        setCurrentPage(newPage);
    };

    // Obținerea cărților pentru pagina curentă
    const currentBooks = books.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const pageCount = Math.ceil(books.length / itemsPerPage);

    return (
        <div className="overflow-x-auto relative shadow-md sm:rounded-lg bg-surface-white">
            <div className="flex justify-between items-center">
                <h2 className="text-base ml-5 mt-5 text-surface-black font-semibold leading-tight">Books Inventory</h2>
            </div>
            <table className="w-full text-sm text-left text-gray-500 mt-5">
                <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                    <tr>
                        <th scope="col" className="py-3 px-6">Book Title</th>
                        <th scope="col" className="py-3 px-6">Author</th>
                        <th scope="col" className="py-3 px-6">ISBN</th>
                        <th scope="col" className="py-3 px-6">Genre</th>
                        <th scope="col" className="py-3 px-6">Status</th>
                        <th scope="col" className="py-3 px-6">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {currentBooks.map((book, index) => (
                        <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <td className="py-4 px-6 flex items-center space-x-3">
                                <img src={book.imageUrl} alt={book.title} style={{ width: '50px', height: '50px', objectFit: 'cover' }} />
                                <span>{truncateString(book.title, 50)}</span>
                            </td>
                            <td className="py-4 px-6">{truncateString(book.author, 50)}</td>
                            <td className="py-4 px-6">{book.isbn}</td>
                            <td className="py-4 px-6">{book.genre}</td>
                            <td className={`py-4 px-6 ${book.bookStatus === 2 ? 'text-red-800' : 'text-green-800'} font-bold`}>
                                {book.bookStatus === 2 ? 'Loaned' : 'Available'}
                            </td>
                            <td className="py-4 px-6">
                                <div className="flex items-center space-x-4">
                                    <div className="group relative">
                                        <PencilIcon onClick={() => handleEdit(book)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                                        <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                                            Edit Book
                                        </span>
                                    </div>
                                    <div className="group relative">
                                        <TrashIcon onClick={() => handleOpenDeleteDialog(book)} className="h-5 w-5 text-red-500 cursor-pointer" />
                                        <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                                            Remove Book
                                        </span>
                                    </div>
                                    <div className="group relative">
                                        <EyeIcon onClick={() => handleViewBook(book)} className="h-5 w-5 text-blue-500 cursor-pointer" />
                                        <span className="absolute w-auto p-2 m-2 min-w-max left-1/2 transform -translate-x-1/2 bottom-full bg-black text-white text-xs rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 ease-in-out">
                                            View Book
                                        </span>
                                    </div>
                                </div>
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
            {selectedBook && (
                <DeleteBookConfirmationDialog
                    open={openDeleteDialog}
                    onClose={handleCloseDeleteDialog}
                    onConfirm={handleConfirmDelete}
                    book={selectedBook}
                />
            )}
            {selectedBook && (
                <EditBookModal
                    open={isEditModalOpen}
                    onClose={() => setIsEditModalOpen(false)}
                    book={selectedBook}
                    onBookUpdated={handleBookUpdated} />
            )}
            {selectedBook && (
                <BookDetailsModalForAdmin
                    open={isDetailsModalOpen}
                    onClose={() => setIsDetailsModalOpen(false)}
                    book={selectedBook}
                />
            )}
        </div>
    );
};

export default BooksTable;
