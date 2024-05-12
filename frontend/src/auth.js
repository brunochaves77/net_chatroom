export const login = async (username, password) => {
  try {
    const response = await fetch("https://localhost:7062/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    if (response.ok) {
      const { token } = await response.json();

      localStorage.setItem("token", token);

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
