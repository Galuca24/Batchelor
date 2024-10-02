import React, { useState, useEffect } from 'react';
import axios from 'axios';
import BookCard from "../../BrowseBooks/components/BookCard";
import BookDetailsModal from '../../BrowseBooks/components/BookDetailsModal';
import { useUser } from "../../../../context/LoginRequired";
import { MagnifyingGlassIcon } from '@heroicons/react/24/outline';

const FavoriteBooks = () => {
    const [books, setBooks] = useState([]);
    const [filteredBooks, setFilteredBooks] = useState([]);
    const [bookSearchTerm, setBookSearchTerm] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [selectedBook, setSelectedBook] = useState(null);
    const [modalOpen, setModalOpen] = useState(false);
    const user = useUser();
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(8);
    const [favorites, setFavorites] = useState(new Set());  
    const [reloadFavorites, setReloadFavorites] = useState(false);

    const pageCount = Math.ceil(filteredBooks.length / itemsPerPage);

    const currentBooks = filteredBooks.slice(
        (currentPage - 1) * itemsPerPage,
        currentPage * itemsPerPage
    );

    const handleToggleFavorite = (bookId, isCurrentlyFavorite) => {
      const newFavorites = new Set(favorites);
      let updatedBooks = [];
  
      if (isCurrentlyFavorite) {
          newFavorites.delete(bookId);
          updatedBooks = books.filter(book => book.bookId !== bookId);
      } else {
          newFavorites.add(bookId);
          updatedBooks = [...books];
      }
  
      setFavorites(newFavorites);
      setBooks(updatedBooks); 
      setFilteredBooks(updatedBooks.filter(book => 
          book.title.toLowerCase().includes(bookSearchTerm.toLowerCase()) || 
          book.author.toLowerCase().includes(bookSearchTerm.toLowerCase())
      )); 
  
      if (selectedBook && selectedBook.bookId === bookId) {
          setSelectedBook({ ...selectedBook, isFavorite: !isCurrentlyFavorite });
      }
  };
  
  
  

  useEffect(() => {
    const fetchFavorites = async () => {
        setLoading(true);
        try {
            const { data } = await axios.get(`https://localhost:7115/api/v1/favourites/get-favourites-by-user/${user.userId}`);
            setBooks(data);
            setFilteredBooks(data);
            setLoading(false);
        } catch (error) {
            console.error('Failed to fetch favorites:', error);
            setError('Failed to fetch favorites');
            setLoading(false);
        }
    };

    fetchFavorites();
}, [user.userId, reloadFavorites]); 


    useEffect(() => {
        const filtered = books.filter(book => book.title.toLowerCase().includes(bookSearchTerm.toLowerCase()) || book.author.toLowerCase().includes(bookSearchTerm.toLowerCase()));
        setFilteredBooks(filtered);
    }, [bookSearchTerm, books]);

    const handleOpenModal = (book) => {
        setSelectedBook({ ...book, isFavorite: true }); 
        setModalOpen(true);
    };

    const handleCloseModal = () => {
      setModalOpen(false);
      setReloadFavorites(prev => !prev); 
  };
  

    const handleBookLoan = (updatedBook) => {
        setBooks(books.map(book => book.bookId === updatedBook.bookId ? updatedBook : book));
        setSelectedBook(updatedBook);  
    };

    if (loading) return <p>Loading books...</p>;
    if (error) return <p>Error loading books: {error}</p>;

    return (
        <div className="mt-12 px-4">
            <h1 className="text-2xl font-bold mb-4">Favorite Books</h1>
            <div className="relative w-full sm:w-1/3 mb-3">
                <div className="absolute inset-y-0 left-0 flex items-center pl-3">
                    <MagnifyingGlassIcon className="w-4 h-4 text-black" />
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
            <div className="grid grid-cols-1 lg:grid-cols-4 md:grid-cols-3 sm:grid-cols-2 gap-0 mt-10 ">
                {currentBooks.map((book, index) => (
                    <BookCard key={book.bookId} book={book} onViewDetails={() => handleOpenModal(book)} />
                ))}
                {selectedBook && (
                    <BookDetailsModal
                        open={modalOpen}
                        onClose={handleCloseModal}
                        book={selectedBook}
                        onBookLoan={handleBookLoan}
                        isFavoriteInitial={true} 
                        onToggleFavorite={handleToggleFavorite} 

                    />
                )}
            </div>
            <div className="flex justify-center p-4 text-black xl:mr-12">
                {[...Array(pageCount).keys()].map(n => (
                    <button
                        key={n + 1}
                        onClick={() => setCurrentPage(n + 1)}
                        className={`px-3 py-1 ${currentPage === n + 1 ? 'text-white bg-surface-dark-green' : 'bg-white'}`}>
                        {n + 1}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default FavoriteBooks;
