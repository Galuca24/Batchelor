import React from 'react';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogTitle from '@mui/material/DialogTitle';
import Button from '@mui/material/Button';

function DeleteBookConfirmationDialog({ open, onClose, onConfirm, book }) {
  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>{"Confirm Delete"}</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Are you sure you want to delete the book "{book?.title}"? This action cannot be undone.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          Cancel
        </Button>
        <Button onClick={() => onConfirm(book.bookId)} color="primary" autoFocus>
          Delete
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default DeleteBookConfirmationDialog;
