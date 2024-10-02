import React, { useState, useEffect } from 'react';
import UserTable from './components/UserTable';
import BooksTable from './components/BooksTable';
import { useUser } from '../../../context/LoginRequired';
import { Navigate } from "react-router-dom";
import { Button } from "@material-tailwind/react";
import Stack from '@mui/material/Stack';
import axios from 'axios';
import { TextField } from '@mui/material';
import AddBookModal from "./components/AddBookModal";
import { MagnifyingGlassIcon } from '@heroicons/react/24/solid';
import GoogleBooksSearchModal from "./components/GoogleBooksSearchModal";
import FinesTableForAdmin from "./components/FinesTableForAdmin";
import LibrivoxSearchModal from "./components/LibrivoxSearchModal";
import AddAudioBookModal from "./components/AddAudioBookModal";
import AudioBooksTable from "./components/AudioBooksTable";

const s3BaseUrl = "https://galabucket.s3.eu-north-1.amazonaws.com/";

const UserManagerPage = () => {
  const [users, setUsers] = useState([]);
  const [books, setBooks] = useState([]);
  const [userSearchTerm, setUserSearchTerm] = useState("");
  const [bookSearchTerm, setBookSearchTerm] = useState("");
  const [currentView, setCurrentView] = useState("UserManager");
  const [openAddBookModal, setOpenAddBookModal] = useState(false);
  const [openGoogleSearchModal, setOpenGoogleSearchModal] = useState(false);
  const [sortType, setSortType] = useState('title'); // Default sort by title
  const [openLibrivoxSearchModal, setOpenLibrivoxSearchModal] = useState(false);
  const [audioBooks, setAudioBooks] = useState([]);
  const [audioBookSearchTerm, setAudioBookSearchTerm] = useState('');
  const [storeView, setStoreView] = useState('Books');


  useEffect(() => {
    const fetchAudioBooks = async () => {
      let audioBooksUrl = 'https://localhost:7115/api/v1/audiobooks/get-all-audiobooks';
      if (audioBookSearchTerm.trim()) {
        audioBooksUrl = `https://localhost:7115/api/v1/audiobooks/search-librivox-books?query=${audioBookSearchTerm}`;
      }

      try {
        const response = await axios.get(audioBooksUrl);
        let audioBooksData = response.data;

        setAudioBooks(audioBooksData);
      } catch (error) {
        console.error(`Failed to fetch audio books:`, error);
      }
    };

    fetchAudioBooks();
  }, [audioBookSearchTerm]);

  const user = useUser();

  if (user.role !== "Admin") {
    return <Navigate to="/dashboard/home" />;
  }

  useEffect(() => {
    fetchUsers();
  }, []);

  const sortedBooks = React.useMemo(() => {
    switch (sortType) {
      case 'title':
        return [...books].sort((a, b) => a.title.localeCompare(b.title));
      case 'author':
        return [...books].sort((a, b) => a.author.localeCompare(b.author));
      case 'genre':
        return [...books].sort((a, b) => a.genre.localeCompare(b.genre));
      case 'status':
        return [...books].sort((a, b) => {
          if (a.bookStatus === b.bookStatus) {
            return 0;
          }
          return a.bookStatus === 2 ? -1 : 1;
        });
      case 'borrowings':
        return [...books].sort((a, b) => b.timesBorrowed - a.timesBorrowed);
      default:
        return books;
    }
  }, [books, sortType]);


  const durationToSeconds = (duration) => {
    const [hours, minutes, seconds] = duration.split(':').map(Number);
    return (hours * 3600) + (minutes * 60) + seconds;
  };

  const secondsToDuration = (seconds) => {
    const hours = Math.floor(seconds / 3600).toString().padStart(2, '0');
    const minutes = Math.floor((seconds % 3600) / 60).toString().padStart(2, '0');
    const secs = (seconds % 60).toString().padStart(2, '0');
    return `${hours}:${minutes}:${secs}`;
  };

  const sortedAudioBooks = React.useMemo(() => {
    switch (sortType) {
      case 'title':
        return [...audioBooks].sort((a, b) => a.title.localeCompare(b.title));
      case 'author':
        return [...audioBooks].sort((a, b) => a.author.localeCompare(b.author));
      case 'genre':
        return [...audioBooks].sort((a, b) => a.genre.localeCompare(b.genre));
      case 'duration':
        return [...audioBooks].sort((a, b) => durationToSeconds(a.duration) - durationToSeconds(b.duration));
      default:
        return audioBooks;
    }
  }, [audioBooks, sortType]);

  const handleOpenAddBookModal = () => setOpenAddBookModal(true);
  const handleCloseAddBookModal = () => setOpenAddBookModal(false);
  const handleOpenLibrivoxSearchModal = () => setOpenLibrivoxSearchModal(true);
  const handleCloseLibrivoxSearchModal = () => setOpenLibrivoxSearchModal(false);

  const handleBookAdded = (newBook) => {
    setBooks(prevBooks => {
      const updatedBooks = [...prevBooks, newBook];
      return updatedBooks;
    });
  };

  const handleDeleteBook = (bookId) => { setBooks(currentBooks => currentBooks.filter(book => book.bookId !== bookId)); };
  const handleDeleteAudioBook = (audioBookId) => {
    setAudioBooks(currentAudioBooks => currentAudioBooks.filter(audioBook => audioBook.audioBookId !== audioBookId));
  };

  const handleBookUpdated = (updatedBook) => {
    setBooks(currentBooks =>
      currentBooks.map(book =>
        book.bookId === updatedBook.bookId ? updatedBook : book
      )
    );
  };

  const handleAudioBookUpdated = (updatedAudioBook) => {
    setAudioBooks(currentAudioBooks =>
      currentAudioBooks.map(audioBook =>
        audioBook.audioBookId === updatedAudioBook.audioBookId ? updatedAudioBook : audioBook
      )
    );
  };

  const handleResultsFetched = (books) => {
    console.log("Salut");
  };


  const fetchUsers = async () => {
    try {
      const response = await axios.get('https://localhost:7115/api/v1/Users');
      const usersData = response.data.users;

      // Apeluri paralele pentru a obține photoUrl pentru fiecare utilizator
      const usersWithPhotos = await Promise.all(usersData.map(async user => {
        try {
          const photoResponse = await axios.get(`https://localhost:7115/api/Cloud/get-user-photo?userId=${user.userId}`);
          const fullPhotoUrl = s3BaseUrl + photoResponse.data.userPhotoUrl;
          return { ...user, photoUrl: fullPhotoUrl };
        } catch (photoError) {
          console.error('Error fetching photo:', photoError);
          return { ...user, photoUrl: "/img/placeholder.png" }; // fallback pentru pozele de profil care nu pot fi încărcate
        }
      }));

      setUsers(usersWithPhotos);
    } catch (error) {
      console.error('Failed to fetch users:', error);
    }
  };



  useEffect(() => {
    const fetchBooks = async () => {
      let booksUrl = 'https://localhost:7115/api/v1/Books/GetAllBooks';
      if (bookSearchTerm.trim()) {
        booksUrl = `https://localhost:7115/api/v1/Books/SearchBooks/${bookSearchTerm}`;
      }

      try {
        const response = await axios.get(booksUrl);
        let booksData = response.data.books || response.data;

        const booksWithBorrowCount = await Promise.all(booksData.map(async (book) => {
          try {
            const borrowCountResponse = await axios.get(`https://localhost:7115/api/v1/books/gettimesborrowed/${book.bookId}`);
            return { ...book, timesBorrowed: borrowCountResponse.data.timesBorrowed };
          } catch (error) {
            console.error(`Failed to fetch borrow count for book ${book.bookId}:`, error);
            return { ...book, timesBorrowed: 0 };
          }
        }));

        setBooks(booksWithBorrowCount);
      } catch (error) {
        console.error(`Failed to fetch books:`, error);
      }
    };

    fetchBooks();
  }, [bookSearchTerm]);

  const handleAudioBookAdded = (newAudioBook) => {
    setAudioBooks((prevAudioBooks) => [...prevAudioBooks, newAudioBook]);
  };



  return (
    <div>
      <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
        <Button
          onClick={() => setCurrentView('UserManager')}
          className={`font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "UserManager" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          User Manager
        </Button>
        <Button
          onClick={() => setCurrentView('StoreManager')}
          className={` font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "StoreManager" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          Store Manager
        </Button>
        <Button
          onClick={() => setCurrentView('FineManager')}
          className={` font-medium w-full sm:w-1/2 md:w-1/4 lg:w-1/5 ${currentView === "FineManager" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
        >
          Fine Manager
        </Button>

      </Stack>

      {currentView === "UserManager" && (
        <>
          <form className="mb-4">  { }
            <label htmlFor="search" className="mb-2 text-sm font-medium sr-only dark:text-white">Search</label>
            <div className="relative">
              <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
                <MagnifyingGlassIcon className="w-4 h-4 fill-black text-black dark:text-black" />
              </div>
              <input
                type="search"
                id="search"
                className="block w-full p-4 pl-10 text-sm xl:w-1/3 md:w-1/3 sm:w-1/3 rounded-lg bg-gray-50 focus:ring-surface-dark-green focus:border-surface-dark-green focus:outline-surface-dark-green dark:text-white"
                placeholder="Search"
                autoComplete="off"
                required
                onChange={(e) => setUserSearchTerm(e.target.value)}
              />

            </div>
          </form>

          <UserTable users={users.filter(user => user.name.toLowerCase().includes(userSearchTerm.toLowerCase()))} onDelete={(userId) => {
            setUsers(users.filter(user => user.userId !== userId));
          }} />
        </>
      )}


{currentView === "StoreManager" && (
  <>
    <div className="flex flex-col md:flex-row justify-between items-center mb-5">
      <div className="relative w-full md:w-1/3 mb-3 md:mb-0">
        <div className="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
          <MagnifyingGlassIcon className="w-4 h-4 text-black dark:text-black" />
        </div>
        <input
          type="search"
          className="block w-full p-4 pl-10 text-sm rounded-lg bg-gray-50 focus:ring-surface-dark-green focus:border-surface-dark-green focus:outline-surface-dark-green dark:text-white"
          placeholder="Search Books"
          autoComplete="off"
          value={storeView === 'Books' ? bookSearchTerm : audioBookSearchTerm}
          onChange={(e) => storeView === 'Books' ? setBookSearchTerm(e.target.value) : setAudioBookSearchTerm(e.target.value)}
        />
      </div>

      <div className=" mx-auto my-3 lg:mx-auto lg:my-3 lg:md:my-0 md:mx-auto md:my-3 md:md:my-0 sm:mx-auto sm:my-3 sm:md:my-0">
        <select
          className="p-2 text-sm rounded-lg bg-gray-50 focus:ring-surface-dark-green focus:border-surface-dark-green"
          value={sortType}
          onChange={(e) => setSortType(e.target.value)}
        >
          <option value="title">Sort by Title</option>
          <option value="author">Sort by Author</option>
          <option value="genre">Sort by Genre</option>
          {storeView === 'Books' && <option value="borrowings">Sort by Times Borrowed</option>}
          {storeView === 'AudioBooks' && <option value="duration">Sort by Duration</option>}
        </select>
      </div>

      <div className="flex flex-col md:flex-row gap-2 w-full md:w-1/3 justify-end">
        <Button
          onClick={handleOpenAddBookModal}
          className="text-white bg-surface-dark-green hover:bg-surface-light-green focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-4 py-2 w-full md:w-auto"
        >
          Add Books
        </Button>
        <Button
          onClick={() => setOpenGoogleSearchModal(true)}
          className="text-white bg-surface-dark-green hover:bg-surface-light-green focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-4 py-2 w-full md:w-auto"
        >
          Search Google Books
        </Button>
        <Button
          onClick={() => setOpenLibrivoxSearchModal(true)}
          className="text-white bg-surface-dark-green hover:bg-surface-light-green focus:ring-4 focus:outline-none font-medium rounded-lg text-sm px-4 py-2 w-full md:w-auto"
        >
          Search Librivox
        </Button>
      </div>
    </div>

    <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
      <Button
        onClick={() => setStoreView('Books')}
        className={`font-medium ${storeView === "Books" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
      >
        Books
      </Button>
      <Button
        onClick={() => setStoreView('AudioBooks')}
        className={`font-medium ${storeView === "AudioBooks" ? 'bg-surface-light-green text-white' : 'bg-surface-dark-green text-surface-white'} hover:bg-surface-dark-green hover:text-white`}
      >
        Audio Books
      </Button>
    </Stack>

    <LibrivoxSearchModal
      open={openLibrivoxSearchModal}
      onClose={handleCloseLibrivoxSearchModal}
      onAudioBookAdded={handleAudioBookAdded}
    />
    <AddBookModal
      open={openAddBookModal}
      onClose={handleCloseAddBookModal}
      onBookAdded={handleBookAdded}
    />
    <GoogleBooksSearchModal
      open={openGoogleSearchModal}
      onClose={() => setOpenGoogleSearchModal(false)}
      onResultsFetched={handleResultsFetched}
      onBookAdded={handleBookAdded}
    />

    {storeView === 'Books' ? (
      <BooksTable
        books={sortedBooks}
        onDelete={handleDeleteBook}
        onUpdate={handleBookUpdated}
        setBooks={setBooks}
      />
    ) : (
      <AudioBooksTable
        audioBooks={sortedAudioBooks}
        onDelete={handleDeleteAudioBook}
        onUpdate={handleAudioBookUpdated}
        setAudioBooks={setAudioBooks}
      />
    )}
  </>
)}



      {currentView === "FineManager" && <FinesTableForAdmin />}


    </div>
  );
};

export default UserManagerPage;
