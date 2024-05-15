export const register = async (username, password) => {
  try {
    const response = await fetch("https://localhost:7062/api/register", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    if (response.ok) {
      return true;
    } else {
      const errorData = await response.json();
      throw new Error(errorData.message || "Failed to register.");
    }
  } catch (error) {
    console.error("Failed to register:", error);
    throw error;
  }
};

export const login = async (username, password) => {
  try {
    const response = await fetch("https://localhost:7062/api/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    if (response.ok) {
      const token = await response.text();

      localStorage.setItem("token", token);

      return token;
    } else {
      throw new Error("Login failed");
    }
  } catch (error) {
    console.error("Failed to login:", error);
    return false;
  }
};

export const logout = async () => {
  try {
    const response = await fetch("https://localhost:7062/api/logout", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
    });

    if (response.ok) {
      localStorage.removeItem("token");
    } else {
      throw new Error("Logout failed");
    }
  } catch (error) {
    console.error("Failed to logout:", error);
    return false;
  }
};

export const isAuthenticated = () => {
  return localStorage.getItem("token") !== null;
};
