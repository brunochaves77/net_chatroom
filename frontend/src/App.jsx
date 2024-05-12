import "bootstrap/dist/css/bootstrap.min.css";
import { Container, Row, Col } from "react-bootstrap";
import { useState } from "react";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import WaitingRoom from "./components/Waitingroom";

function App() {
  const [conn, setConnection] = useState();

  const joinChatRoom = async (chatroom) => {
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

  const leaveChatRoom = () => {
    setConnection(null);
    setMessages([]);
    setRoomName(null);
  };

  return (
    <>
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
          <Row>
            <Col>
              {!conn ? <WaitingRoom joinChatRoom={joinChatRoom} /> : ""}
            </Col>
          </Row>
        </Container>
      </main>
    </>
  );
}

export default App;
