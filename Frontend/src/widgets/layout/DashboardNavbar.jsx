import { useLocation, Link } from "react-router-dom";
import {
  Navbar,
  Typography,
  Button,
  IconButton,
  Breadcrumbs,
  Menu,
  MenuHandler,
  MenuList,
  ListItem
} from "@material-tailwind/react";
import {
  UserCircleIcon,
  BellIcon,
  ClockIcon,
  Bars3Icon,
  CheckIcon
} from "@heroicons/react/24/solid";
import {
  useMaterialTailwindController,
  setOpenSidenav,
} from "@/context";
import { useEffect, useState } from "react";
import api from "@/services/api";
import { toast } from "react-toastify";
import { useUser } from "@/context/LoginRequired.jsx";

export function DashboardNavbar() {
  const [controller, dispatch] = useMaterialTailwindController();
  const { userId, username, token } = useUser();
  const { openSidenav } = controller;
  const { pathname } = useLocation();
  const [layout, page] = pathname.split("/").filter((el) => el !== "");
  const [notifications, setNotifications] = useState([]);
  const [hasUnreadNotifications, setHasUnreadNotifications] = useState(false);

  // useEffect(() => {
  //   (async () => {
  //     const inboxItems = await fetchNotifications(userId, token);
  //     inboxItems.sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));

  //     setNotifications(inboxItems);
  //     // Verifică dacă există notificări necitite și actualizează starea
  //     const unreadExists = inboxItems.some(item => !item.isRead);
  //     setHasUnreadNotifications(unreadExists);
  //   })();
  // }, [userId, token]);

   useEffect(() => {
    const fetchNotificationsAndSetState = async () => {
      const inboxItems = await fetchNotifications(userId, token);
      inboxItems.sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));
      setNotifications(inboxItems);
      
      const unreadExists = inboxItems.some(item => !item.isRead);
      setHasUnreadNotifications(unreadExists);
    };
  
    const intervalId = setInterval(fetchNotificationsAndSetState, 100); // 5000ms = 5 secunde
  
    return () => clearInterval(intervalId);
  }, [userId, token]);


  async function fetchNotifications(userId, token) {
    try {
      const response = await api.get(`/api/v1/InboxItems/get-unread-by-user-id/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data.inboxItems;
    } catch (error) {
      console.log(`Error while getting notifications: ${error.response.data}`);
    }
  }

  async function markAsRead(e, notification) {
    e.stopPropagation();
    try {
      const response = await api.put(`/api/v1/inboxitems/update-is-read/${notification.inboxItemId}`, {
        isRead: true,
      });
      if (response.status !== 204) {
        throw new Error(response);
      }
      setNotifications(prev => {
        const updated = prev.map(notif => notif.inboxItemId === notification.inboxItemId ? {...notif, isRead: true} : notif);
        // Verifică din nou pentru notificări necitite
        setHasUnreadNotifications(updated.some(notif => !notif.isRead));
        return updated;
      });
    } catch (error) {
      toast.error("Couldn't update notification");
    }
  }
  


  return (
    <Navbar
      color={"transparent"}
      className={"rounded-xl  transition-all px-0 py-1"}
      fullWidth
    >
      <div className="flex flex-col-reverse justify-between gap-6 md:flex-row md:items-center" >
        <div className="capitalize text-surface-black" >
          <Breadcrumbs
            className={"bg-transparent p-0 transition-all"}
          >
            <Link to={`/${layout}`}>
              <Typography
                variant="small"
                className="font-normal opacity-60 transition-all text-surface-dark-green hover:opacity-100 hover:text-surface-light-green"
              >
                {layout}
              </Typography>
            </Link>
            <Typography
              variant="small"
              className="font-normal text-surface-light "
            >
              {page}
            </Typography>
          </Breadcrumbs>
        </div>
        <div className="flex items-center" fill="green">
          <IconButton
            variant="text"
            color="blue-gray"
            className="grid xl:hidden"
            onClick={() => setOpenSidenav(dispatch, !openSidenav)}
          >
            <Bars3Icon strokeWidth={3} className="h-6 w-6 text-blue-gray-500" />
          </IconButton>
          <Link to="/dashboard/profile">
            <Button
              variant="text"
              color="blue-gray"
              className="hidden items-center gap-1  text-surface-black px-4 xl:flex normal-case"
              fill="green"
            >
              <UserCircleIcon className="h-5 w-5 text-primary" fill="green" />
              {username}
            </Button>
            <IconButton
              variant="text"
              color="blue-gray"
              className="grid xl:hidden"
              fill="green"
            >
              <UserCircleIcon className="h-5 w-5 text-blue-gray-500" fill="green" />
            </IconButton>
          </Link>


          <Menu>
            <MenuHandler>
              <div className="relative">
                <IconButton variant="text" color="blue-gray">
                  <BellIcon className="h-5 w-5 text-surface-dark-green" />
                </IconButton>
                {hasUnreadNotifications && (
                  <span className="absolute top-0 right-0 block h-2 w-2 rounded-full bg-red-500"></span>
                )}
              </div>
            </MenuHandler>
            <MenuList className="border-0 bg-surface-white max-w-sm max-h-[80vh] minimal-scrollbar text-surface-green">
              {notifications.length === 0 ?
                <ListItem disabled className="text-black opacity-70" style={{ color: "black" }}>
                  <strong>No new notifications</strong>
                </ListItem>
                :
                notifications.map((notification, index) =>
                  <ListItem className="flex flex-col items-start w-full md:w-[20rem]" key={`notif-${index}`}>
                    <Typography variant="small" className="mb-1 font-normal " >
                      <strong>{notification.message}</strong>
                    </Typography>

                    <div className="flex justify-between w-full">
                      <Typography variant="small" className="flex items-center gap-1 text-xs font-normal opacity-60">
                        <ClockIcon className="h-3.5 w-3.5 text-surface-dark-green" /> {formatDate(notification.createdDate)}
                      </Typography>
                      {notification.isRead === false ?
                        <Button variant="text" color="red" size="sm" className="text-xs px-2 py-1 opacity-70 font-normal"
                          onClick={(e) => markAsRead(e, notification)}
                        >
                          Mark as read
                        </Button>
                        :
                        <Typography variant="small" className="flex items-center gap-1 text-xs font-normal opacity-60">
                          <CheckIcon className="h-3.5 w-3.5" /> Read
                        </Typography>
                      }
                    </div>
                  </ListItem>
                )
              }
            </MenuList>
          </Menu>
        </div>
      </div>
    </Navbar>
  );
}

DashboardNavbar.displayName = "/src/widgets/layout/dashboard-navbar.jsx";

function formatDate(date) {
  return new Intl.DateTimeFormat("ro", {
    day: "numeric",
    month: "numeric",
    year: "numeric",
    hour: "numeric",
    minute: "numeric",
    hour12: false,
    timeZone: "Europe/Bucharest",
  }).format(new Date(date));
}

export default DashboardNavbar;
