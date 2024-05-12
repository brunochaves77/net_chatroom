import { Route, Navigate } from "react-router-dom";

const PrivateRoute = ({ component: Component, isAuthenticated, ...rest }) => (
  <Route
    {...rest}
    element={
      isAuthenticated ? <Component {...rest} /> : <Navigate to="/login" />
    }
  />
);

export default PrivateRoute;
