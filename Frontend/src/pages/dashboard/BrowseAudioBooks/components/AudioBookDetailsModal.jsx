import React, { useEffect, useRef, useState } from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material';
import { createTheme, ThemeProvider } from '@mui/material/styles';
import { useUser } from '../../../../context/LoginRequired';
import AudioBookReviewSection from './AudioBookReviewSection';

const AudioBookDetailsModal = ({ open, onClose, audioBook }) => {
  const user = useUser();
  const [isExpanded, setIsExpanded] = useState(false);
  const theme = createTheme({
    palette: {
      primary: {
        main: '#006400',
      },
    },
  });

  const audioRefs = useRef([]);

  const getTotalDurationInSeconds = () => {
    return audioBook.chapters.reduce((total, chapter) => total + parseDuration(chapter.duration), 0);
  };

  const getTotalListenedTimeInSeconds = () => {
    return audioBook.chapters.reduce((total, chapter) => {
      const savedTime = localStorage.getItem(`audio_${user.userId}_${chapter.chapterId}`);
      return total + (savedTime ? parseFloat(savedTime) : 0);
    }, 0);
  };

  useEffect(() => {
    if (open) {
      audioBook.chapters.forEach((chapter, index) => {
        const savedTime = localStorage.getItem(`audio_${user.userId}_${chapter.chapterId}`);
        if (savedTime && audioRefs.current[index]) {
          const audioElement = audioRefs.current[index];
          audioElement.currentTime = parseFloat(savedTime);

          setTimeout(() => {
            if (audioElement) {
              audioElement.currentTime = parseFloat(savedTime);
            }
          }, 0);
        }
      });
    }
  }, [open, audioBook.chapters, user.userId]);

  const handleTimeUpdate = (chapterId, index) => {
    if (audioRefs.current[index]) {
      localStorage.setItem(`audio_${user.userId}_${chapterId}`, audioRefs.current[index].currentTime);
    }
  };

  const handleOnPlay = (chapterId, index) => {
    const savedTime = localStorage.getItem(`audio_${user.userId}_${chapterId}`);
    if (savedTime && audioRefs.current[index]) {
      audioRefs.current[index].currentTime = parseFloat(savedTime);
    }
  };

  const formatTime = (seconds) => {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const remainingSeconds = Math.floor(seconds % 60);

    if (hours > 0) {
      return `${hours}:${minutes < 10 ? '0' : ''}${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
    } else {
      return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
    }
  };

  const parseDuration = (duration) => {
    const parts = duration.split(':');
    let seconds = 0;
    if (parts.length === 3) {
      seconds += parseInt(parts[0], 10) * 3600;
      seconds += parseInt(parts[1], 10) * 60;
      seconds += parseInt(parts[2], 10);
    } else if (parts.length === 2) {
      seconds += parseInt(parts[0], 10) * 60;
      seconds += parseInt(parts[1], 10);
    }
    return seconds;
  };

  const totalDurationInSeconds = getTotalDurationInSeconds();
  const totalListenedTimeInSeconds = getTotalListenedTimeInSeconds();
  const progressPercentage = Math.min((totalListenedTimeInSeconds / totalDurationInSeconds) * 100, 100).toFixed(2);

  const toggleDescription = () => {
    setIsExpanded(!isExpanded);
  };

  const getDescription = () => {
    const words = audioBook.description.split(' ');
    const wordCount = words.length;
    if (wordCount > 100) {
      if (isExpanded) {
        return audioBook.description;
      }
      return words.slice(0, 100).join(' ') + '...'; // Afișează primele 100 de cuvinte + "..."
    }
    return audioBook.description;
  };

  return (
    <ThemeProvider theme={theme}>
      <Dialog
        open={open}
        onClose={onClose}
        aria-labelledby="audiobook-details-title"
        fullWidth
        maxWidth="md"
        PaperProps={{
          sx: { minHeight: '80vh', maxWidth: '60vw', width: '60vw', height: '30vw' }
        }}
      >
        <DialogTitle id="audiobook-details-title" className="bg-gray-50 text-lg font-semibold">
          {audioBook.title}
        </DialogTitle>
        <DialogContent dividers className="bg-white p-4 flex flex-col lg:flex-row">
          <img
            src={audioBook.imageUrl}
            alt={audioBook.title}
            className="w-full h-auto object-cover rounded-lg lg:w-5/12 xl:w-5/12 lg:mr-6"
          />
          <div className="flex-1 min-w-0 lg:mt-0 mt-6">
            <p className="text-xl mb-2 text-gray-800 font-medium lg:mt-2"><strong>Author:</strong> {audioBook.author}</p>
            <p className="text-xl mb-2 text-gray-800 font-medium lg:mt-2"><strong>Genre:</strong> {audioBook.genre}</p>
            <p className="text-xl mb-2 text-gray-600"><strong>Description:</strong> </p>
            <div className="mb-4 overflow-y-auto max-h-32 text-sm text-gray-600 border p-2 rounded break-words">
              {getDescription()}
              {audioBook.description.split(' ').length > 100 && (
                <Button
                  className="absolute bottom-0 right-0"
                  onClick={toggleDescription}
                  size="small"
                >
                  {isExpanded ? 'Less' : 'More'}
                </Button>
              )}
            </div>
            <div className="w-full mt-0">
              <h3 className="text-lg font-semibold mb-1">Chapters ({progressPercentage}% completed)</h3>
              <div className="overflow-auto max-h-48" style={{ scrollbarWidth: 'thin' }}>
                <ul className="list-none">
                  {audioBook.chapters.map((chapter, index) => {
                    const savedTime = localStorage.getItem(`audio_${user.userId}_${chapter.chapterId}`);
                    const savedTimeFormatted = savedTime ? formatTime(parseFloat(savedTime)) : '0:00';
                    const chapterDurationInSeconds = parseDuration(chapter.duration);
                    const isChapterComplete = savedTime && parseFloat(savedTime) >= chapterDurationInSeconds;

                    return (
                      <li key={index} className="mb-4">
                        <p className="text-lg font-medium">
                          {chapter.chapterName} - {savedTimeFormatted}/{formatTime(chapterDurationInSeconds)}
                          {isChapterComplete && <span> ✔️</span>}
                        </p>
                        <audio
                          controls
                          className="w-full"
                          ref={(el) => audioRefs.current[index] = el}
                          onTimeUpdate={() => handleTimeUpdate(chapter.chapterId, index)}
                          onPlay={() => handleOnPlay(chapter.chapterId, index)}
                        >
                          <source src={chapter.audioUrl} type="audio/mpeg" />
                          Your browser does not support the audio element.
                        </audio>
                      </li>
                    );
                  })}
                </ul>
              </div>
            </div>
            <AudioBookReviewSection audioBookId={audioBook.audioBookId} />
          </div>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose} color="primary">
            Close
          </Button>
        </DialogActions>
      </Dialog>
    </ThemeProvider>
  );
};

export default AudioBookDetailsModal;
