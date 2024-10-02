import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { toast } from 'react-toastify';
import AddBookModal from './AddBookModal';
import { createTheme, ThemeProvider } from '@mui/material/styles';

function GoogleBooksSearchModal({ open, onClose, onBookAdded }) {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [selectedBook, setSelectedBook] = useState(null);
  const [isAddBookModalOpen, setIsAddBookModalOpen] = useState(false);

  const openAddBookModalWithBook = (book) => {
    setSelectedBook(book);
    setIsAddBookModalOpen(true);
  };

  function truncateString(str, num) {
    if (str.length > num) {
      return str.slice(0, num) + "...";
    } else {
      return str;
    }
  }


  const handleSearch = async () => {
    if (!searchTerm.trim()) return;

    try {
      const response = await axios.get(`https://localhost:7115/api/v1/Books/SearchGoogleBooks?query=${searchTerm}`);
      if (response.data) {
        setSearchResults(response.data);
        toast.success("Books fetched successfully!");
      } else {
        toast.error("No books found.");
      }
    } catch (error) {
      console.error('Failed to fetch books:', error);
      toast.error("Error fetching books.");
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
      <Dialog open={open} onClose={onClose} fullWidth maxWidth="md" className='ml-5'>
        <DialogTitle>Search Books from Google</DialogTitle>
        <DialogContent>
          <TextField
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            margin="dense"
            id="search"
            name="search"
            label="Search Term"
            fullWidth
            variant="outlined"
            required
          />
          <Button
            onClick={handleSearch}
            variant="contained"
            color="primary"
            sx={{
              marginTop: 2,
              width: '100%',
              backgroundColor: theme.palette.primary.main,
              color: 'white',
              '&:hover': {
                backgroundColor: theme.palette.primary.dark,
              }
            }}
          >
            Search
          </Button>
          {searchResults.length > 0 && (
            <div style={{ marginTop: 20 }}>
              {searchResults.map((book, index) => (
                <div key={index} style={{ display: 'flex', alignItems: 'center', marginBottom: 10 }}>
                  <img src={book.imageUrl} alt={book.title} style={{ width: 100, height: 150, marginRight: 20 }} />
                  <div style={{ flexGrow: 1 }}>
                    <h3>{truncateString(book.title,73)}</h3>
                    <p>{truncateString(book.author,73)}</p>
                    <p>ISBN: {book.googleBooksId}</p>
                  </div>
                  <Button variant="contained" color="primary" onClick={() => openAddBookModalWithBook(book)}>
                    + Add to Catalog
                  </Button>
                </div>
              ))}
              <AddBookModal
                open={isAddBookModalOpen}
                onClose={() => setIsAddBookModalOpen(false)}
                onBookAdded={onBookAdded}
                initialData={selectedBook}
              />
            </div>

          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Close</Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
}

export default GoogleBooksSearchModal;
