import "bootstrap/dist/css/bootstrap.min.css";
import { Container, Row, Col, Button } from "react-bootstrap";
import { useState } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import WaitingRoom from "./components/Waitingroom";
import ChatRoom from "./components/Chatroom";
import Login from "./components/Login";
import PrivateRoute from "./components/PrivateRoute";
import {
  isAuthenticated,
  login as loginService,
  logout as logoutService,
} from "./auth";

function App() {
  const [conn, setConnection] = useState();
  const [messages, setMessages] = useState([]);
  const [roomName, setRoomName] = useState();
  const [isLoggedIn, setIsLoggedIn] = useState(isAuthenticated());

  const joinChatRoom = async (chatroom) => {
    setRoomName(chatroom);
    try {
      const conn = new HubConnectionBuilder()
        .withUrl("https://localhost:7062/chatHub")
        .configureLogging(LogLevel.Information)
        .build();

      conn.on("JoinChatRoom", (msg) => {
        console.log("msg: ", msg);
      });

      conn.on("ReceiveMessage", (username, msg) => {
        console.log(msg);
        setMessages((messages) => [...messages, { username, msg }]);
      });

      await conn.start();
      await conn.invoke("JoinChatRoom", chatroom);

      setConnection(conn);
    } catch (e) {
      console.log(e);
    }
  };

  const sendMessage = async (message) => {
    try {
      await conn.invoke("SendMessage", roomName, userName, message);
    } catch (e) {
      console.log(e);
    }
  };

  const leaveChatRoom = () => {
    setConnection(null);
    setMessages([]);
    setRoomName(null);
  };

  const login = async (username, password) => {
    try {
      const success = await loginService(username, password);
      if (success) {
        setIsLoggedIn(true);
      } else {
        alert("Failed to login. Please check your credentials and try again.");
      }
    } catch (error) {
      console.error("Failed to login:", error);
    }
  };

  const logout = async () => {
    try {
      await logoutService();
      setIsLoggedIn(false);
    } catch (error) {
      console.error("Failed to logout:", error);
    }
  };

  return (
    <Router>
      <main>
        <div className="bg-primary py-3">
          <Container>
            <Row>
              <Col>
                <h1 className="text-white text-center">Netrooms</h1>
              </Col>
            </Row>
          </Container>
        </div>
        <Container className="mt-4">
          <Routes>
            <Route
              path="/login"
              element={<Login login={login} />}
            />
            <Route
              path="/"
              element={
                isLoggedIn ? (
                  <WaitingRoom joinChatRoom={joinChatRoom} />
                ) : (
                  <Navigate to="/login" />
                )
              }
            />
            <Route
              path="/chatroom"
              element={
                isLoggedIn ? (
                  <ChatRoom
                    messages={messages}
                    sendMessage={sendMessage}
                    roomName={roomName}
                    leaveChatRoom={leaveChatRoom}
                  />
                ) : (
                  <Navigate to="/login" />
                )
              }
            />
          </Routes>
          {isLoggedIn && (
            <div className="d-flex justify-content-end mt-4">
              <Button
                variant="danger"
                onClick={logout}
              >
                Logout
              </Button>
            </div>
          )}
        </Container>
      </main>
    </Router>
  );
}

export default App;
