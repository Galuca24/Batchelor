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

function AddAudioBookModal({ open, onClose, onAudioBookAdded, initialData }) {
  const [formData, setFormData] = useState({
    title: '',
    isbn: '',
    author: '',
    genre: '',
    description: '',
    audioUrl: '',
    duration: '',
    imageUrl: '',
    chapters: []
  });

  const generateRandomISBN = () => {
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    let result = '';
    for (let i = 0; i < 12; i++) {
      result += characters.charAt(Math.floor(Math.random() * characters.length));
    }
    return result;
  };

  useEffect(() => {
    if (initialData) {
      setFormData({
        title: initialData.title || '',
        isbn: initialData.isbn || generateRandomISBN(),
        author: initialData.author || '',
        genre: initialData.genre || '',
        description: initialData.description || '',
        audioUrl: initialData.urlAudio || '',
        duration: initialData.duration || '',
        imageUrl: initialData.imageUrl || '',
        chapters: initialData.chapters || []
      });
    }
  }, [initialData]);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleChapterChange = (index, e) => {
    const newChapters = formData.chapters.map((chapter, idx) => {
      if (idx === index) {
        return { ...chapter, [e.target.name]: e.target.value };
      }
      return chapter;
    });
    setFormData({ ...formData, chapters: newChapters });
  };

  const handleAddChapter = () => {
    setFormData({ ...formData, chapters: [...formData.chapters, { chapterId: '', chapterName: '', audioUrl: '', duration: '' }] });
  };

  const handleRemoveChapter = (index) => {
    const newChapters = formData.chapters.filter((_, idx) => idx !== index);
    setFormData({ ...formData, chapters: newChapters });
  };

  const handleAddAudioBookWithImageLoad = async (event) => {
    event.preventDefault();
    const img = new Image();
    img.src = formData.imageUrl || placeholderImage;

    img.onload = async () => {
      try {
        const response = await axios.post('https://localhost:7115/api/v1/audiobooks/add-audio-book', formData);
        if (response.data.success) {
          toast.success("Audio Book added successfully!");
          onAudioBookAdded({ ...response.data.audioBook, imageUrl: formData.imageUrl || placeholderImage });
          onClose();
        } else {
          toast.error("Failed to add audio book: " + response.data.message);
        }
      } catch (error) {
        console.error('Failed to add audio book:', error);
        toast.error("Error adding audio book.");
      }
    };

    img.onerror = () => {
      formData.imageUrl = placeholderImage;
      img.src = formData.imageUrl;
    };
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
      <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
        <form onSubmit={handleAddAudioBookWithImageLoad}>
          <DialogTitle>Add a New Audio Book</DialogTitle>
          <DialogContent dividers style={{ maxHeight: '70vh', overflow: 'auto' }}>
            <TextField value={formData.title} onChange={handleChange} margin="dense" id="title" name="title" label="Title" fullWidth variant="outlined" required />
            <TextField value={formData.isbn} onChange={handleChange} margin="dense" id="isbn" name="isbn" label="ISBN" fullWidth variant="outlined" required />
            <TextField value={formData.author} onChange={handleChange} margin="dense" id="author" name="author" label="Author" fullWidth variant="outlined" required />
            <TextField value={formData.genre} onChange={handleChange} margin="dense" id="genre" name="genre" label="Genre" fullWidth variant="outlined" required />
            <TextField value={formData.imageUrl} onChange={handleChange} margin="dense" id="imageUrl" name="imageUrl" label="Image" fullWidth variant="outlined" required />
            <TextField value={formData.audioUrl} onChange={handleChange} margin="dense" id="audioUrl" name="audioUrl" label="Audio URL" fullWidth variant="outlined" required />
            <TextField value={formData.duration} onChange={handleChange} margin="dense" id="duration" name="duration" label="Duration" fullWidth variant="outlined" required />
            <TextField value={formData.description} onChange={handleChange} margin="dense" id="description" name="description" label="Description" fullWidth variant="outlined" required multiline rows={4} />
            <h4>Chapters</h4>
            <div style={{ maxHeight: '200px', overflow: 'auto', marginBottom: '1em' }}>
              {formData.chapters.map((chapter, index) => (
                <div key={index} style={{ marginBottom: '1em' }}>
                  <TextField value={chapter.chapterName} onChange={(e) => handleChapterChange(index, e)} margin="dense" id="chapterName" name="chapterName" label="Chapter Name" fullWidth variant="outlined" required />
                  <TextField value={chapter.audioUrl} onChange={(e) => handleChapterChange(index, e)} margin="dense" id="audioUrl" name="audioUrl" label="Chapter Audio URL" fullWidth variant="outlined" required />
                  <TextField value={chapter.duration} onChange={(e) => handleChapterChange(index, e)} margin="dense" id="duration" name="duration" label="Chapter Duration" fullWidth variant="outlined" required />
                  <Button variant="outlined" className="bg-surface-dark-green" onClick={() => handleRemoveChapter(index)}>Remove Chapter</Button>
                </div>
              ))}
            </div>
          </DialogContent>
          <DialogActions>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="submit" color="primary">Add Audio Book</Button>
          </DialogActions>
        </form>
      </Dialog>
    </ThemeProvider>
  );
}

export default AddAudioBookModal;
