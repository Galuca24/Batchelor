import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material';
import axios from 'axios';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import ReviewSection from "../../BrowseBooks/components/ReviewSection"

const BookDetailsModalForAdmin = ({ open, onClose, book }) => {
  const [isExpanded, setIsExpanded] = useState(false);
  const [timesBorrowed, setTimesBorrowed] = useState(0); // State to store times borrowed

  useEffect(() => {
    if (book && open) {
      fetchTimesBorrowed(book.bookId);
    }
  }, [book, open]); 

  const fetchTimesBorrowed = async (bookId) => {
    try {
      const response = await axios.get(`https://localhost:7115/api/v1/books/gettimesborrowed/${bookId}`);
      if (response.data.success) {
        setTimesBorrowed(response.data.timesBorrowed);
      } else {
        throw new Error(response.data.message || "Failed to fetch times borrowed");
      }
    } catch (error) {
      console.error("Error fetching times borrowed:", error);
    }
  };

  const toggleDescription = () => {
    setIsExpanded(!isExpanded);
  };

  const getDescription = () => {
    const words = book.description.split(' ');
    if (words.length > 100) {
      if (isExpanded) {
        return book.description;
      }
      return words.slice(0, 100).join(' ') + '...';
    }
    return book.description;
  };

  const theme = createTheme({
    palette: {
      primary: {
        main: '#006400',
      },
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <Dialog
        open={open}
        onClose={onClose}
        aria-labelledby="book-details-title"
        fullWidth
        maxWidth="md"
        PaperProps={{
          sx: { minHeight: '80vh', maxWidth: '60vw', width: '100%', height: 'auto' }
        }}
      >
        <DialogTitle id="book-details-title" className="bg-gray-50 text-lg font-semibold">
          {book.title}
        </DialogTitle>
        <DialogContent dividers className="bg-white p-4 flex flex-col lg:flex-row">
          <img
            src={book.imageUrl}
            alt={book.title}
            className="w-full h-auto object-cover rounded-lg lg:w-5/12 xl:w-5/12 lg:mr-6"
          />
          <div className="flex-1 min-w-0 lg:mt-0 mt-6">
            <p className="text-xl mb-8 text-gray-800 font-medium lg:mt-4"><strong>Author:</strong> {book.author}</p>
            <p className="text-xl mb-8 text-gray-800 font-medium lg:mt-4"><strong>Genre:</strong> {book.genre}</p>
            <p className="text-xl mb-8 text-gray-800 font-medium lg:mt-4"><strong>ISBN:</strong> {book.isbn}</p>
            <p className="text-xl mb-8 text-gray-800 mt-2"><strong>Status:</strong>
              <span className={book.bookStatus === 2 ? 'text-red-800' : 'text-green-800'}>
                {book.bookStatus === 2 ? 'Loaned' : 'Available'}
              </span>
            </p>
            <p className="text-xl mb-8 text-gray-800 mt-2"><strong>Times Borrowed:</strong> {timesBorrowed}</p>
            <p className="text-xl mb-3 text-gray-600"><strong>Description:</strong> </p>
            <div className="mb-8 overflow-y-auto max-h-32 text-sm text-gray-600 border p-2 rounded break-words relative">
              {getDescription()}
              {book.description.split(' ').length > 100 && (
                <Button
                  className="absolute bottom-0 right-0"
                  onClick={toggleDescription}
                  size="small"
                >
                  {isExpanded ? 'Less' : 'More'}
                </Button>
              )}
            </div>
            <ReviewSection bookId={book.bookId} />

          </div>

        </DialogContent>
        <DialogActions className="bg-gray-50">
          <Button onClick={onClose} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
};

export default BookDetailsModalForAdmin;
