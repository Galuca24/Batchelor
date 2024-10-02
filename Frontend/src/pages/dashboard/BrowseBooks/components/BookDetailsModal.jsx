import React, { useState, useEffect } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, TextField, Typography } from '@mui/material';
import { useUser } from "../../../../context/LoginRequired";
import { toast } from 'react-toastify';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { HeartIcon } from '@heroicons/react/24/solid';
import { HeartIcon as HeartIconFilled } from '@heroicons/react/24/solid'; // Iconița umplută pentru cărțile favorite
import axios from "axios";
import Rating from '@mui/material/Rating';
import { sendNotification } from '../../../../services/notifications/sendNotification';
import ReviewSection from './ReviewSection';

const BookDetailsModal = ({ open, onClose, book, onBookLoan, isFavoriteInitial, onToggleFavorite }) => {
  const user = useUser();
  const [loanSuccess, setLoanSuccess] = useState(false);
  const [isExpanded, setIsExpanded] = useState(false);
  const [isBookLoaned, setIsBookLoaned] = useState(book.bookStatus === 2);
  const [isFavorite, setIsFavorite] = useState(isFavoriteInitial);
  const [votesCount, setVotesCount] = useState(0);
  const [userRating, setUserRating] = useState(0);
  const [averageRating, setAverageRating] = useState(0);

  const fetchVotesCount = async () => {
    try {
      const response = await axios.get(`https://localhost:7115/api/v1/rating/getvotescount/${book.bookId}`);
      setVotesCount(response.data || 0);
    } catch (error) {
      console.error('Error fetching votes count:', error);
      setVotesCount(0);
    }
  };

  useEffect(() => {
    const fetchRatingAverage = async () => {
      try {
        const { data } = await axios.get(`https://localhost:7115/api/v1/rating/getbookratingsaverage/${book.bookId}`);
        const avgRating = Number(data) || 0;
        setAverageRating(avgRating);
        setUserRating(avgRating);  
      } catch (error) {
        console.error('Error fetching average rating:', error);
        setAverageRating(0);
      }
    };

    if (book) {
      fetchRatingAverage();
      fetchVotesCount();
    }
  }, [book]);

  const fetchRatingAverage = async () => {
    try {
      const { data } = await axios.get(`https://localhost:7115/api/v1/rating/getbookratingsaverage/${book.bookId}`);
      const avgRating = Number(data) || 0;
      setAverageRating(avgRating);
      setUserRating(avgRating);  
    } catch (error) {
      console.error('Error fetching average rating:', error);
      setAverageRating(0);
    }
  };

  const handleRatingChange = async (event, newRating) => {
    try {
      const response = await axios.post('https://localhost:7115/api/v1/rating/giveratingtobook', {
        bookId: book.bookId,
        userId: user.userId,
        value: newRating
      });
      if (response.data.success) {
        setUserRating(newRating);  // Actualizează ratingul dat de utilizator
        fetchRatingAverage();  // Re-fetch the average to update immediately
        fetchVotesCount();
        toast.success('Rating updated successfully!');
      } else {
        toast.error('Failed to update rating.');
      }
    } catch (error) {
      console.error('Error updating rating:', error);
      toast.error('Failed to update rating.');
    }
  };

  useEffect(() => {
    setIsFavorite(isFavoriteInitial);
  }, [isFavoriteInitial]);


  const toggleDescription = () => {
    setIsExpanded(!isExpanded);
  };

  const fetchFavorites = async () => {
    try {
      const { data } = await axios.get(`https://localhost:7115/api/v1/favourites/get-favourites-by-user/${user.userId}`);
      const favoriteBookIds = new Set(data.map(book => book.bookId));
      setIsFavorite(favoriteBookIds);
    } catch (error) {
      console.error('Failed to fetch favorites:', error);
    }
  };
  const handleToggleFavorite = async () => {
    const newFavoriteStatus = !isFavorite;
    console.log("Current isFavorite state:", isFavorite);
    console.log("Attempting to set new favorite status to:", newFavoriteStatus);

    const method = newFavoriteStatus ? 'POST' : 'DELETE';
    const endpoint = `https://localhost:7115/api/v1/favourites/${newFavoriteStatus ? 'add-book-to-favourite' : 'remove-book-from-favourite'}`;
    console.log("Endpoint:", endpoint, "Method:", method);

    try {
      const response = await axios({
        method: method,
        url: endpoint,
        headers: {
          'Content-Type': 'application/json'
        },
        data: JSON.stringify({
          userId: user.userId,
          bookId: book.bookId
        })
      });
      console.log("Response from toggle favorite:", response);

      if (response.data.success) {
        setIsFavorite(newFavoriteStatus);
        onToggleFavorite(book.bookId, newFavoriteStatus);
        console.log("Successfully toggled favorite. New status:", newFavoriteStatus);
        toast.success(response.data.message);
        fetchFavorites();
      } else {
        console.error("Failed to toggle favorite with message:", response.data.message);
        toast.error("Failed to toggle favorite status.");
      }
    } catch (error) {
      console.error('Error toggling favorite status:', error);
      toast.error("Failed to toggle favorite status.");
    }
  };





  const getDescription = () => {
    const words = book.description.split(' ');
    const wordCount = words.length;
    if (wordCount > 100) {
      if (isExpanded) {
        return book.description;
      }
      return words.slice(0, 100).join(' ') + '...'; // Afișează primele 100 de cuvinte + "..."
    }
    return book.description;
  };

  const handleLoanBook = async () => {
    if (book.bookStatus === 2) {
      toast.error("This book is already loaned.");
      return;
    }

    try {
      const response = await fetch('https://localhost:7115/api/v1/books/loanbook', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          bookId: book.bookId,
          userId: user.userId
        })
      });
      const data = await response.json();
      if (response.ok) {
        const bookResponse = await axios.get(`https://localhost:7115/api/v1/books/getbookbyid/${book.bookId}`);
        const bookTitle = bookResponse.data.title;

        const adminResponse = await axios.get('https://localhost:7115/api/v1/users/admin');
        const adminUserId = adminResponse.data.users[0].userId;

        if (user.userId !== adminUserId) {
          sendNotification(adminUserId, `${user.username} has loaned book ${bookTitle}`);
        }


        setIsBookLoaned(true);  // Setăm cartea ca fiind împrumutată
        onBookLoan({ ...book, bookStatus: 2 });
        toast.success("Loan successful!");
      } else {
        toast.error(data.message);
        console.error(data.message);
      }
    } catch (error) {
      toast.error("An error occurred during the loan process");
      console.error('Error:', error);
    }
  };

  const theme = createTheme({
    palette: {
      primary: {
        main: '#006400', // un verde întunecat
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
          sx: { minHeight: '80vh', maxWidth: '60vw', width: '60vw', height: '30vw' }
        }}
      >
        <DialogTitle id="book-details-title" className="bg-gray-50 text-lg font-semibold">
          {book.title}
        </DialogTitle>
        <DialogContent dividers className="bg-white p-4 flex flex-col lg:flex-row ">
          <img
            src={book.imageUrl}
            alt={book.title}
            className="w-full h-auto object-cover rounded-lg  lg:w-5/12 xl:w-5/12 lg:mr-6"
          />
          <div className="flex-1 min-w-0 lg:mt-0 mt-6 ">


            <p className="text-xl mb-8 text-gray-800 font-medium lg:mt-4"><strong>Author:</strong> {book.author}</p>
            <p className="text-xl mb-8 text-gray-800 font-medium lg:mt-4"><strong>Genre:</strong> {book.genre}</p>

            <p className="text-xl mb-8 text-gray-800 mt-2"><strong>Status:</strong>  <span className={book.bookStatus === 2 ? 'text-red-800' : 'text-green-800'}>
              {book.bookStatus === 2 ? 'Loaned' : 'Available'}
            </span></p>
            <p className="text-xl mb-3 text-gray-600"><strong>Description:</strong> </p>
            <div className="mb-8 overflow-y-auto max-h-32 text-sm text-gray-600 border p-2 rounded break-words">
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

            <div className="flex items-center mb-8 flex-col sm:flex-row ">
              <strong className="text-xl text-gray-800 font-medium mr-1 ml-2 ">Rating:</strong>
              <div className="flex items-center flex-col mt-6 xl:mt-0 lg:mt-0 md:mt-0 sm:mt-0 sm:flex-row sm:mb-0">
                <Rating
                  name="book-rating"
                  value={userRating}
                  onChange={handleRatingChange}
                  precision={0.5}
                  size="large"
                  className="text-2xl"
                  onMouseLeave={() => setUserRating(averageRating)}
                />
                <span className="ml-2">Votes: {votesCount}</span>
              </div>
            </div>



            <Button
              onClick={handleLoanBook}
              variant="contained"
              color="primary"
              disabled={book.bookStatus === 2}
              className="flex w-full justify-center lg:w-full"
            >
              Loan Book
            </Button>
            <ReviewSection bookId={book.bookId} />

          </div>

        </DialogContent>
        
        <DialogActions >
          <Button onClick={onClose} color="primary">
            Close
          </Button>
        </DialogActions>
        <div className="absolute top-0 right-0 p-4">
          <button onClick={handleToggleFavorite}>
            {isFavorite ? (
              <HeartIconFilled className="h-6 w-6 text-red-500" />
            ) : (
              <HeartIcon className="h-6 w-6 text-gray-400 hover:text-red-500" />
            )}
          </button>

        </div>


      </Dialog>
    </ThemeProvider>
  );
};

export default BookDetailsModal;