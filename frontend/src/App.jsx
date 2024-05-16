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
import Register from "./components/Register";
import {
  isAuthenticated,
  login as loginService,
  logout as logoutService,
  baseUrl,
} from "./auth";

function App() {
  const [conn, setConnection] = useState();
  const [messages, setMessages] = useState([]);
  const [roomName, setRoomName] = useState();
  const [isLoggedIn, setIsLoggedIn] = useState(isAuthenticated());

  const joinChatRoom = async (chatroom) => {
    setRoomName(chatroom);
    try {
      const token = localStorage.getItem("token");

      const conn = new HubConnectionBuilder()
        .withUrl(`${baseUrl}/chatHub`, {
          accessTokenFactory: () => token,
        })
        .configureLogging(LogLevel.Information)
        .build();

      conn.on("GetMessages", async (roomId) => {
        console.log("msg nova: ", roomId);
        try {
          var url = `${baseUrl}/api/messages/latest-by-room-id/${roomId}`;
          console.log("test", url);
          const response = await fetch(url, {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
            },
          });

          if (response.ok) {
            const data = await response.json();
            console.log(data);

            data.sort((a, b) => {
              return new Date(a.receivedAt) - new Date(b.receivedAt);
            });

            data.reverse();

            setMessages([]);

            setMessages(
              data.map((message) => ({
                id: message.id,
                name: message.username,
                msg: message.message,
                receivedAt: message.receivedAt,
              }))
            );

            console.log("messagesss", messages);
          } else {
            throw new Error("Get messages failed");
          }
        } catch (error) {
          console.error("Failed to get messages:", error);
          return false;
        }
      });

      conn.on("ReceiveMessage", (user, msg) => {
        var name = user.userName;
        setMessages((messages) => [...messages, { name, msg }]);
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
      await conn.invoke("SendMessage", message);
      console.log("eviou", message);
    } catch (e) {
      console.log(e);
    }
  };

  const login = async (username, password) => {
    try {
      const success = await loginService(username, password);
      if (success) {
        console.log("Logou-se");
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
      setConnection(null);
      setMessages([]);
      setRoomName(null);
    } catch (error) {
      console.error("Failed to logout:", error);
    }
  };

  const leaveChatRoom = () => {
    setConnection(null);
    setMessages([]);
    setRoomName(null);
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
              path="/register"
              element={<Register />}
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
