import React, { useState } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { toast } from 'react-toastify';
import AddAudioBookModal from './AddAudioBookModal';
import { createTheme, ThemeProvider } from '@mui/material/styles';

function LibrivoxSearchModal({ open, onClose, onAudioBookAdded }) {
  const [searchTerm, setSearchTerm] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [selectedAudioBook, setSelectedAudioBook] = useState(null);
  const [isAddAudioBookModalOpen, setIsAddAudioBookModalOpen] = useState(false);

  const openAddAudioBookModalWithAudioBook = (audioBook) => {
    setSelectedAudioBook(audioBook);
    setIsAddAudioBookModalOpen(true);
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
      const response = await axios.get(`https://localhost:7115/api/v1/audiobooks/search-librivox-books?query=${searchTerm}`);
      if (response.data) {
        setSearchResults(response.data);
        toast.success("Audio books fetched successfully!");
      } else {
        toast.error("No audio books found.");
      }
    } catch (error) {
      console.error('Failed to fetch audio books:', error);
      toast.error("Error fetching audio books.");
    }
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
      <Dialog open={open} onClose={onClose} fullWidth maxWidth="md">
        <DialogTitle>Search Audio Books from Librivox</DialogTitle>
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
              {searchResults.map((audioBook, index) => (
                <div key={index} style={{ display: 'flex', alignItems: 'center', marginBottom: 10 }}>
                  <img src={audioBook.imageUrl} alt={audioBook.title} style={{ width: 100, height: 150, marginRight: 20 }} />
                  <div style={{ flexGrow: 1 }}>
                    <h3>{truncateString(audioBook.title, 73)}</h3>
                    <p>{truncateString(audioBook.author, 73)}</p>
                    <p>Duration: {audioBook.duration}</p>
                  </div>
                  <Button variant="contained" color="primary" onClick={() => openAddAudioBookModalWithAudioBook(audioBook)}>
                    + Add to Catalog
                  </Button>
                </div>
              ))}
              <AddAudioBookModal
                open={isAddAudioBookModalOpen}
                onClose={() => setIsAddAudioBookModalOpen(false)}
                onAudioBookAdded={onAudioBookAdded}
                initialData={selectedAudioBook}
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

export default LibrivoxSearchModal;
