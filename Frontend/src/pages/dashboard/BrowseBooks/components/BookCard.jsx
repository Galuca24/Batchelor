import React from 'react';
import Button from '@mui/material/Button';
import { height } from '@mui/system';
import { createTheme, ThemeProvider } from '@mui/material/styles';

const theme = createTheme({
    palette: {
      primary: {
        main: '#006400', // un verde întunecat
      },
    },
  });
const BookCard = ({ book, onViewDetails }) => {
    return (
        <ThemeProvider theme={theme}>
        <div className="xl:w-60 p-4 rounded-lg shadow-sm bg-transparent flex flex-col items-center text-center hover:bg-surface-darker-white hover:shadow-lg transition duration-300 ease-in-out hover:border hover:border-gray-300">
            <img
                src={book.imageUrl || '/path/to/default/image.jpg'}
                alt={book.title}
                className="h-72 w-48 object-cover rounded-lg mb-2"
            />
            <div className="mt-auto w-full">
                <h3 className="text-lg font-semibold truncate">{book.title}</h3>
                <p className="text-gray-600 text-sm">{book.author}</p>
            </div>
            <Button 
                variant="outlined"
                size="small"
                style={{ marginTop: '10px' }}
                onClick={onViewDetails}
            >
                View Details
            </Button>
        </div>
        </ThemeProvider>
    );
};

export default BookCard;
