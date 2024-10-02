import { Routes, Route } from "react-router-dom";
import routes from "@/routes";

export function Succes() {
  return (
    <div className="relative min-h-screen bg-surface-white w-full">
      <Routes>
        {routes.map(
          ({ layout, pages }) =>
            layout === "succes" &&
            pages.map(({ path, element }) => (
              <Route exact path={path} element={element} />
            ))
        )}
      </Routes>
    </div>
  );
}

Succes.displayName = "/src/layout/Succes.jsx";

export default Succes;