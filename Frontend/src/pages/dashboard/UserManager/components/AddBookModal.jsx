import React, { useState, useEffect } from 'react';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import TextField from '@mui/material/TextField';
import axios from 'axios';
import { toast } from 'react-toastify';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const placeholderImage = "https://via.placeholder.com/150"; 

function AddBookModal({ open, onClose, onBookAdded, initialData }) {
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
    if (initialData) {
      setFormData({
        title: initialData.title || '',
        isbn: initialData.googleBooksId || '',
        author: initialData.author || '',
        genre: initialData.genre || '',
        description: initialData.description || '',
        imageUrl: initialData.imageUrl || '',
        bookStatus: initialData.bookStatus || 1
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleAddBookWithImageLoad = async (event) => {
    event.preventDefault();
    const img = new Image();
    img.src = formData.imageUrl || placeholderImage;
  
    img.onload = async () => {
      try {
        const response = await axios.post('https://localhost:7115/api/v1/Books/AddBook', formData);
        if (response.data.success) {
          toast.success("Book added successfully!");
          onBookAdded({ ...response.data.book, imageUrl: formData.imageUrl || placeholderImage }); // Include imaginea originală sau placeholder-ul
          onClose();
        } else {
          toast.error("Failed to add book: " + response.data.message);
        }
      } catch (error) {
        console.error('Failed to add book:', error);
        toast.error("Error adding book.");
      }
    };
  
    img.onerror = () => {
      formData.imageUrl = placeholderImage; // Setează URL-ul la placeholder
      img.src = formData.imageUrl; // Încearcă din nou cu imaginea placeholder
    };
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
      <form onSubmit={handleAddBookWithImageLoad}>
        <DialogTitle>Add a New Book</DialogTitle>
        <DialogContent>
          <TextField value={formData.title} onChange={handleChange} margin="dense" id="title" name="title" label="Title" fullWidth variant="outlined" required />
          <TextField value={formData.isbn} onChange={handleChange} margin="dense" id="isbn" name="isbn" label="ISBN" fullWidth variant="outlined" required />
          <TextField value={formData.author} onChange={handleChange} margin="dense" id="author" name="author" label="Author" fullWidth variant="outlined" required />
          <TextField value={formData.genre} onChange={handleChange} margin="dense" id="genre" name="genre" label="Genre" fullWidth variant="outlined" required />
          <TextField value={formData.imageUrl} onChange={handleChange} margin="dense" id="imageUrl" name="imageUrl" label="Image" fullWidth variant="outlined" required/>
          <TextField value={formData.description} onChange={handleChange} margin="dense" id="description" name="description" label="Description" fullWidth variant="outlined" required multiline rows={4} />
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button type="submit" color="primary">Add Book</Button>
        </DialogActions>
      </form>
    </Dialog>
    </ThemeProvider>
  );
}

export default AddBookModal;