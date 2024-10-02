import React from "react";
import BorrowedBooksCard from "./BorrowedBooksCount";
import BooksCountCard from "./BooksCountCard";
import ReturnedBooksCard from "./ReturnedBooksCard";
import MissingBooksCard from "./MissingBooksCard"; 
import ReturnedOverdueBooksCard from "./ReturnedOverdueBooksCard";
import PendingFeesCard from "./PendingFeesCard";
import BooksChart from "./BooksChart";
import OverdueHistory from "./OverdueHistory";
import RecentCheckouts from "./RecentCheckouts";
import UserGreeting from "./UserGreeting";
import {useUser} from "../../../../context/LoginRequired"

export function HomeForAdmin() {
  const user = useUser();

  return (
    <div className="mt-12 text-surface-light-green">
      <div className="ml-24">
            <UserGreeting userName={user.name} />  {/* Adaugă componenta de salut aici, schimbă numele după caz */}
            </div>
      <div className="grid  w-full grid-cols-1 sm:grid-cols-2 md:grid-cols-2 xl:grid-cols-3 xl:gap-6 px-4 md:px-6 lg:px-8 xl:px-10 mx-auto">
        {/* Carduri specifice pentru Admin */}
        <BorrowedBooksCard />
        <ReturnedBooksCard />
        <BooksCountCard />
        <MissingBooksCard />
        <ReturnedOverdueBooksCard />
        <PendingFeesCard />
      </div>
      <div className="gap-5 flex flex-wrap justify-between xl:mt-10">
        <div className="w-full custom:w-full custom-xl:w-1/2 xl:w-1/2 xl:h-1/2 p-4 bg-surface-white gap-x-6">
          <BooksChart />
        </div>
        <div className="w-full custom:w-full custom:mr-0 custom-xl:mr-14 custom-xl:w-5/12 xl:mr-14 xl:w-6/12 xl:h-1/2">
      <OverdueHistory />
    </div>
      </div>
      <div className="mt-10">
        <RecentCheckouts />
      </div>
    </div>
  );
}

export default HomeForAdmin;
