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
      
      console.log("Logou");

      return true;
    } else {
      throw new Error("Login failed");
    }
  } catch (error) {
    console.error("Failed to login:", error);
    return false;
  }
};

export const logout = () => {
  localStorage.removeItem("token");
};

export const isAuthenticated = () => {
  return localStorage.getItem("token") !== null;
};
