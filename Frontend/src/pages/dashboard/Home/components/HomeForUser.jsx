import React, { useState, useEffect } from "react";
import axios from 'axios';
import Slider from "react-slick";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

import BookCardForHomePage from './BookCardForHomePage';
import BorrowedBooksCard from "./BorrowedBooksCount";
import BooksCountCard from "./BooksCountCard";
import ReturnedBooksCard from "./ReturnedBooksCard";
import ReturnedOverdueBooksCard from "./ReturnedOverdueBooksCard";
import PendingFeesCard from "./PendingFeesCard";
import RatingGivenByUser from "./RatingGivenByUser";
import UserGreeting from './UserGreeting';
import { useUser } from "../../../../context/LoginRequired"
export function HomeForUser() {
  const [books, setBooks] = useState([]);
  const [recomandations, setRecomandations] = useState([]);

  const user = useUser();
  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const response = await axios.get('https://localhost:7115/api/v1/books/getmostborrowedbooksinlast7days');
        setBooks(response.data);
      } catch (error) {
        console.error('Failed to fetch books', error);
      }
    };

    fetchBooks();
  }, []);

  useEffect(() => {
    const fetchRecomandations = async () => {
      try {
        const userId = user.userId;
        if (!userId) {
          console.log('User ID is undefined.');
          return; 
        }
        const url = `https://localhost:7115/api/v1/books/get-recomandations-by-user/${userId}`;
        console.log(`Fetching recommendations with URL: ${url}`);
        const response = await axios.get(url);
        setRecomandations(response.data);
        console.log('Recommendations fetched:', response.data);
      } catch (error) {
        console.error('Failed to fetch recommendations', error);
      }
    };

    if (user && user.userId) {
      fetchRecomandations();
    }
  }, [user]); // Reactualizează când user se schimbă, inclusiv userId-ul



  const settings = {
    dots: false,
    infinite: true,
    speed: 500,
    slidesToShow: 5,
    slidesToScroll: 1,

    responsive: [
      {
        breakpoint: 1295,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
        }
      },
      {
        breakpoint: 1140,
        settings: {
          slidesToShow: 5,
          slidesToScroll: 1,
        }
      },
      {
        breakpoint: 760,
        settings: {
          slidesToShow: 4,
          slidesToScroll: 1,
          centerPadding: '100px'

        }
      },
      {
        breakpoint: 590,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 500,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 340,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
    ]
  };

  return (
    <div className="mt-12 px-4 md:px-6 lg:px-8 xl:px-10 mx-auto xl:mr-10">
      <div className="ml-10">
        <UserGreeting userName={user.name} />  {/* Adaugă componenta de salut aici, schimbă numele după caz */}
      </div>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-2 xl:grid-cols-3 xl:gap-6 xl:mr-8">
        <BooksCountCard />
        <BorrowedBooksCard />
        <ReturnedBooksCard />
        <ReturnedOverdueBooksCard />
        <PendingFeesCard />
        <RatingGivenByUser />
        <div className="w-full xl:w-full lg:w-auto lg:mx-10  col-span-full col-span xl:ml-10">
          <h2 className="text-lg font-bold mb-1 text-black">Top Choices This Week</h2>
          <Slider {...settings}>
            {books.map((book) => (
              <div key={book.bookId}>
                <BookCardForHomePage book={book} />
              </div>
            ))}
          </Slider>
        </div>

        <div className="w-full xl:w-full lg:w-auto lg:mx-10 col-span-full xl:ml-10">
          <h2 className="text-lg font-bold mb-1 text-black">Recommended for You</h2>
          <Slider {...settings}>
            {recomandations.map((book) => (
              <div key={book.bookId}>
                <BookCardForHomePage book={book} />
              </div>
            ))}
          </Slider>
        </div>
      </div>
    </div>
  );
}

export default HomeForUser;
