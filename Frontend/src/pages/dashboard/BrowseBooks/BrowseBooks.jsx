import React, { useState, useEffect } from 'react';
import axios from 'axios';
import BookCard from "./components/BookCard";
import { MagnifyingGlassIcon } from '@heroicons/react/24/outline';
import BookDetailsModal from './components/BookDetailsModal';
import { useUser } from "../../../context/LoginRequired";
import { sendNotification } from '../../../services/notifications/sendNotification';
import Slider from 'react-slick';

const BrowseBooks = () => {
    const [books, setBooks] = useState([]);
    const [bookSearchTerm, setBookSearchTerm] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [selectedBook, setSelectedBook] = useState(null);
    const [modalOpen, setModalOpen] = useState(false);
    const [favorites, setFavorites] = useState(new Set());  // Folosim un Set pentru a stoca ID-uri de cărți favorite
    const user =useUser();
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(8);
    
    const pageCount = Math.ceil(books.length / itemsPerPage);

 
    const changePage = (newPage) => {
        setCurrentPage(newPage);
    };

    // Obține cărțile pentru pagina curentă
    const currentBooks = books.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    useEffect(() => {
      const fetchFavorites = async () => {
          try {
              const { data } = await axios.get(`https://localhost:7115/api/v1/favourites/get-favourites-by-user/${user.userId}`);
              const favoriteBookIds = new Set(data.map(book => book.bookId));
              setFavorites(favoriteBookIds);
          } catch (error) {
              console.error('Failed to fetch favorites:', error);
          }
      };

      fetchFavorites();
  }, [user.userId]);
  
  const fetchFavorites = async () => {
    try {
        const { data } = await axios.get(`https://localhost:7115/api/v1/favourites/get-favourites-by-user/${user.userId}`);
        const favoriteBookIds = new Set(data.map(book => book.bookId));
        setFavorites(favoriteBookIds);
    } catch (error) {
        console.error('Failed to fetch favorites:', error);
    }
};
const handleToggleFavorite = (bookId, isCurrentlyFavorite) => {
    const newFavorites = new Set(favorites); 
    if (isCurrentlyFavorite) {
        newFavorites.delete(bookId);
    } else {
        newFavorites.add(bookId);
    }
    setFavorites(newFavorites);
    if (selectedBook && selectedBook.bookId === bookId) {
      setSelectedBook({ ...selectedBook, isFavorite: !isCurrentlyFavorite });
    }
  };
  



const handleOpenModal = (book) => {
  const isFavorite = favorites.has(book.bookId); 
  const normalizedBook = {
      ...book,
      isFavorite: isFavorite, 
      bookStatus: book.status || book.bookStatus 
  };
  delete normalizedBook.status; 
  setSelectedBook(normalizedBook);
  setModalOpen(true);
};


  
    
const handleCloseModal = () => {
  setModalOpen(false);
  fetchFavorites(); 
};


      const handleBookLoan = (updatedBook) => {
        setBooks(books.map(book => book.bookId === updatedBook.bookId ? updatedBook : book));
        setSelectedBook(updatedBook);  
      };

    useEffect(() => {
        const fetchBooks = async () => {
            setLoading(true);
            let booksUrl = bookSearchTerm.trim() 
              ? `https://localhost:7115/api/v1/Books/SearchBooks/${bookSearchTerm}`
              : 'https://localhost:7115/api/v1/Books/GetAllBooks';

            try {
                const response = await axios.get(booksUrl);
                setBooks(response.data.books || response.data); 
                setLoading(false);
            } catch (err) {
                setError('Failed to fetch books');
                setLoading(false);
                console.error(err);
            }
        };

        const delayDebounceFn = setTimeout(() => {
            fetchBooks();
        }, 500); 

        return () => clearTimeout(delayDebounceFn); 
    }, [bookSearchTerm]);

    if (loading) return <p>Loading books...</p>;
    if (error) return <p>Error loading books: {error}</p>;

    return (
        <div className="mt-12 px-4">
            <h1 className="text-2xl font-bold mb-4">Available Books</h1>
            <div className="relative w-full sm:w-1/3 mb-3">
                <div className="absolute inset-y-0 left-0 flex items-center pl-3">
                <MagnifyingGlassIcon className="w-4 h-4  text-black dark:text-black" />
                </div>
                <input
                    type="search"
                    className="block w-full p-4 pl-10 text-sm xl:w-96 md:w-96 sm:w-96 rounded-lg bg-gray-50 focus:ring-surface-dark-green focus:border-surface-dark-green focus:outline-surface-dark-green dark:text-white"
                    placeholder="Search Books"
                    autoComplete="off"
                    value={bookSearchTerm}
                    onChange={(e) => setBookSearchTerm(e.target.value)}
                />
            </div>
            <div className="grid grid-cols-1 lg:grid-cols-4 md:grid-cols-3 sm:grid-cols-2 gap-0 mt-10 "> {/* Ajustat gap-ul aici */}
            {currentBooks.map((book,index) => (
        <BookCard key={book.bookId} book={book} onViewDetails={() => handleOpenModal(book)} />
      ))}
      {selectedBook && (
        <BookDetailsModal
        open={modalOpen}
        onClose={handleCloseModal}
        book={selectedBook}
        onBookLoan={handleBookLoan}
        isFavoriteInitial={favorites.has(selectedBook.bookId)}
        onToggleFavorite={handleToggleFavorite} 
    />
      )}
    </div>
    <div className="flex justify-center p-4 text-black xl:mr-12">
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

export default BrowseBooks;