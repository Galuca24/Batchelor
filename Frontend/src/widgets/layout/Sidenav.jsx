import PropTypes from "prop-types";
import { Link, NavLink, useLocation } from "react-router-dom";
import { XMarkIcon } from "@heroicons/react/24/outline";
import {
  RectangleStackIcon,
} from "@heroicons/react/24/solid";
import {
  Button,
  IconButton,
  Typography,
} from "@material-tailwind/react";
import { useMaterialTailwindController, setOpenSidenav } from "@/context";
import { useUser } from "../../context/LoginRequired";
import React, { useContext } from "react";


export function Sidenav({ brandImg, brandName, routes }) {
  const [controller, dispatch] = useMaterialTailwindController();
  const { openSidenav } = controller;
  const user = useUser();

  return (
    <aside
      className={`bg-surface-off-white ${openSidenav ? "translate-x-0" : "-translate-x-80"}
        fixed inset-0 z-50 my-4 ml-4 h-[calc(100vh-32px)] w-64 rounded-xl transition-transform duration-300 xl:translate-x-0`}
    >
      <div className="relative">
        <Link to="/" className="py-5 flex flex-col items-center justify-center">
          <img src={brandImg} alt="logo" className="h-16 mb-2" />
          <Typography variant="h3" color="black" className="w-min text-xl whitespace-nowrap">
            {brandName}
          </Typography>
        </Link>
        <IconButton
          variant="text"
          color="gray"
          size="sm"
          ripple={false}
          className="absolute right-0 top-0"
          onClick={() => setOpenSidenav(dispatch, false)}
        >
          <XMarkIcon strokeWidth={2.5} className="h-5 w-5 text-surface-white" />
        </IconButton>
      </div>
      <div className="m-4">
      {routes.map(({ layout, pages }, key) => (
  <ul key={key} className="flex flex-col gap-1">
    {pages.filter(page => {
      // Exclude paginile de autentificare dacă utilizatorul este autentificat
      if (page.authPage) return false;
      // Exclude paginile admin-only pentru non-admini
      if (page.adminOnly) return user.role === 'Admin';
      // Exclude paginile user-only pentru non-useri
      if (page.userOnly) return user.role === 'User';
      // Include pagina pentru toți ceilalți utilizatori
      return true;
    }).map(({ icon, name, path }) => (
      <li key={name} className="mb-4 last:mb-0">
        <NavLink to={`/${layout}${path}`}>
          {({ isActive }) => (
            <Button className={`flex items-center gap-2 px-4 py-2 ${isActive ? 'bg-surface-light-green' : 'bg-surface-dark-green'}`} fullWidth>
              {icon}
              <Typography color="inherit" className="font-medium capitalize">
                {name}
              </Typography>
            </Button>
          )}
        </NavLink>
      </li>
    ))}
  </ul>
))}

      </div>
    </aside>
  );
}

Sidenav.defaultProps = {
  brandImg: "/img/logo1.png",
  brandName: "Smart Library",
};

Sidenav.propTypes = {
  brandImg: PropTypes.string,
  brandName: PropTypes.string,
  routes: PropTypes.arrayOf(PropTypes.object).isRequired,
};

Sidenav.displayName = "/src/widgets/layout/sidenav.jsx";

export default Sidenav;
