import React, { useState } from "react";
import { Form, Button, Col, Row, Container } from "react-bootstrap";

const WaitingRoom = ({ joinChatRoom }) => {
  const [username, setUsername] = useState("");
  const [chatroom, setChatroom] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    joinChatRoom(chatroom);
  };

  return (
    <Container className="border p-4 rounded">
      <h2 className="text-center mb-4">Join Chatroom</h2>
      <Form onSubmit={handleSubmit}>
        <Row className="mb-3">
          <Col>
            <Form.Control
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="w-50 mx-auto"
            />
          </Col>
        </Row>
        <Row className="mb-3">
          <Col>
            <Form.Control
              placeholder="ChatRoom"
              value={chatroom}
              onChange={(e) => setChatroom(e.target.value)}
              className="w-50 mx-auto"
            />
          </Col>
        </Row>
        <Row className="justify-content-center">
          <Col xs="auto">
            <Button
              variant="success"
              type="submit"
              className="w-100"
            >
              Join
            </Button>
          </Col>
        </Row>
      </Form>
    </Container>
  );
};

export default WaitingRoom;
