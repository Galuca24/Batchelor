import React, { useEffect, useState } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { toast } from 'react-toastify';
import { createTheme, ThemeProvider } from '@mui/material/styles';

function EditBookModal({ open, onClose, book, onBookUpdated }) {
  const [formData, setFormData] = useState({
    title: '',
    isbn: '',
    author: '',
    genre: '',
    description: '',
    imageUrl: '',
    bookStatus: 1
  });

  useEffect(() => {
    if (book) {
      setFormData({
        title: book.title || '',
        isbn: book.isbn || '',
        author: book.author || '',
        genre: book.genre || '',
        description: book.description || '',
        imageUrl: book.imageUrl || '',
        bookStatus: book.bookStatus || 1
      });
    }
  }, [book]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      const response = await axios.put(`https://localhost:7115/api/v1/Books/UpdateBook?bookId=${book.bookId}`, formData);
      console.log("Response data:", response.data); // Adaugă acest log pentru a verifica datele
      if (response.status === 200) {
        toast.success("Book updated successfully!");
       
        onBookUpdated(response.data);
        onClose();
      } else {
        const errorMessage = response.data.message || "An unknown error occurred";
        toast.error("Failed to update book: " + errorMessage);
      }
    } catch (error) {
      console.error('Failed to update book:', error);
      toast.error("Error updating book.");
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
    <Dialog open={open} onClose={onClose}>
      <form onSubmit={handleSubmit}>
        <DialogTitle>Edit Book</DialogTitle>
        <DialogContent>
          <TextField value={formData.title} onChange={handleChange} margin="dense" id="title" name="title" label="Title" fullWidth variant="outlined" required />
          <TextField value={formData.isbn} onChange={handleChange} margin="dense" id="isbn" name="isbn" label="ISBN" fullWidth variant="outlined" required />
          <TextField value={formData.author} onChange={handleChange} margin="dense" id="author" name="author" label="Author" fullWidth variant="outlined" required />
          <TextField value={formData.genre} onChange={handleChange} margin="dense" id="genre" name="genre" label="Genre" fullWidth variant="outlined" required />
          <TextField value={formData.imageUrl} onChange={handleChange} margin="dense" id="imageUrl" name="imageUrl" label="Image URL" fullWidth variant="outlined" required/>
          <TextField value={formData.description} onChange={handleChange} margin="dense" id="description" name="description" label="Description" fullWidth variant="outlined" required multiline rows={4} />
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" color="primary">Update Book</Button>
        </DialogActions>
      </form>
    </Dialog>
    </ThemeProvider>
  );
}

export default EditBookModal;
