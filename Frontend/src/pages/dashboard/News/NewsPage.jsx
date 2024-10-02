import React, { useState, useEffect } from 'react';
import axios from 'axios';

const formatDate = (date) => {
  const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
  return new Date(date).toLocaleDateString("en-US", options);
};

function NewsPage() {
  const [news, setNews] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage] = useState(8);

  useEffect(() => {
    const fetchNews = async () => {
      try {
        const response = await axios.get('https://localhost:7115/api/v1/news');
        const sortedNews = response.data.sort((a, b) => new Date(b.publishedAt) - new Date(a.publishedAt));
        setNews(sortedNews);
      } catch (error) {
        console.error('Failed to fetch news', error);
      }
    };
    fetchNews();
  }, []);

  const pageCount = Math.ceil(news.length / itemsPerPage);

  const changePage = (newPage) => {
    setCurrentPage(newPage);
  };

  const currentNews = news.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  return (
    <div className="mt-12 px-4 md:px-6 lg:px-8 xl:px-10 mx-auto">
      <div className="mb-6">
        <h1 className="text-5xl font-bold text-black">News and <span className="text-green-600">Events</span></h1>
        <p className="text-xl text-gray-600">{formatDate(new Date())}</p>
      </div>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {currentNews.map((item, index) => (
          <div key={index} className="bg-surface-darker-white shadow-md rounded-lg overflow-hidden flex flex-col mb-4">
            {item.urlToImage && <img src={item.urlToImage} alt={item.title} className="w-full h-48 object-cover" />}
            <div className="p-4 flex flex-col flex-grow">
              <h3 className="text-md font-semibold mb-2">{item.title}</h3>
              <p className="text-sm text-gray-500 mb-2">{new Date(item.publishedAt).toLocaleDateString()}</p>
              <p className="text-sm mb-2 flex-grow">{item.description}</p>
              <div className="flex justify-start mt-4">
                <a href={item.url} target="_blank" rel="noopener noreferrer" className="text-surface-dark-green underline self-end">
                  Read more
                </a>
              </div>
            </div>
          </div>
        ))}
      </div>
      <div className="flex justify-center p-4 text-black xl:mr-12">
        {[...Array(pageCount).keys()].map(n => (
          <button
            key={n + 1}
            onClick={() => changePage(n + 1)}
            className={`px-3 py-1 ${currentPage === n + 1 ? 'text-white bg-surface-dark-green' : 'bg-white'}`}>
            {n + 1}
          </button>
        ))}
      </div>
    </div>
  );
}

export default NewsPage;
