import {
  HomeIcon,
  UserCircleIcon,
  TableCellsIcon,
  InformationCircleIcon,
  ServerStackIcon,
  RectangleStackIcon,
  UsersIcon,
  BuildingLibraryIcon,
  NewspaperIcon,
  ChatBubbleLeftEllipsisIcon
} from "@heroicons/react/24/solid";
import { Home, Profile, BrowseBooks, UserManger,UserLibraryPage,UserChats,NewsPage,ChatPage,BrowseAudioBooks } from "./pages/dashboard/index";
import { SignIn, SignUp } from "@/pages/auth";
import ResetPassword from "./pages/auth/ResetPassword";
import VerifyResetCode from "./pages/auth/VerifyResetCode";
import SendResetCode from "./pages/auth/SendResetCode";
import { BookOpenIcon } from "@heroicons/react/24/solid";
import {SuccesPage,CancelPage} from "./pages/succes/index";
import { FaHeadphones } from "react-icons/fa";

const icon = {
  className: "w-5 h-5 text-inherit",
};

export const routes = [
  {
    
    
    layout: "dashboard",
    pages: [    
      {
        icon: <HomeIcon {...icon} />,
        name: "dashboard",
        path: "/home",
        element: <Home />,
      },
      {
        icon: <BookOpenIcon {...icon} />,
        name: "browse books",
        path: "/browse-books",
        element: <BrowseBooks />,
      },
      {
        icon: <FaHeadphones {...icon} />,
        name: "Browse-Audio-Books",
        path: "/browse-audio-books",
        element: <BrowseAudioBooks />,
      },
      {
        icon: <ChatBubbleLeftEllipsisIcon {...icon} />,
        name: "Chat-Assistant",
        path: "/chat-assistant",
        element: <ChatPage />,
      },
      {
        icon: <NewspaperIcon {...icon} />,
        name: "News",
        path: "/news",
        element: <NewsPage />,
      },
      {
        icon: <UserCircleIcon {...icon} />,
        name: "profile",
        path: "/profile/:userId?",
        dynamic: true,
        element: <Profile />,
        authPage: true
      },
      
      {
        icon: <BuildingLibraryIcon {...icon} />,
        name: "user-library",
        path: "/user-library",
        element: <UserLibraryPage />,
        userOnly: false,  // Proprietate nouă
      },
      
      {
        icon: <UsersIcon {...icon} />,
        name: "user manager",
        path: "/user-manager",
        element: <UserManger />,
        adminOnly: true, 
      }
    ],
  },
  {
    title: "auth pages",
    layout: "auth",
    pages: [
      {
        icon: <ServerStackIcon {...icon} />,
        name: "sign in",
        path: "/sign-in",
        element: <SignIn />,
         authPage: true

      },
      {
        icon: <RectangleStackIcon {...icon} />,
        name: "sign up",
        path: "/sign-up",
        element: <SignUp />,
        authPage: true
      },
      {
        icon: <InformationCircleIcon {...icon} />,
        name: "forgot password",
        path: "/forgot-password",
        element: <SendResetCode />,
        authPage: true
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "verify reset",
        path: "/verify-reset",
        element: <VerifyResetCode />,
        authPage: true
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "reset password",
        path: "/reset-password",
        element: <ResetPassword />,
        authPage: true
      }
    ],
  },
  {
    layout: "succes",
    pages: [
      { path: '/succes/fineId?', 
      element: <SuccesPage />,
      authPage: true
       },
       { path: '/cancel/fineId?', 
      element: <CancelPage />,
      authPage: true
       }
    ]
  }
  
];

export default routes;
