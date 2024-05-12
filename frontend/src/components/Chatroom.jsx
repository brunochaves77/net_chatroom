import React from "react";
import { Col, Row, Container, Button } from "react-bootstrap";
import MessageContainer from "./MessageContainer";
import SendMessageForm from "./SendMessageForm";

const ChatRoom = ({ messages, sendMessage, roomName, leaveChatRoom }) => (
  <Container className="border p-4 rounded">
    <Row className="px-5 py-3">
      <Col>
        <h2 className="text-center mb-0">ChatRoom: {roomName}</h2>
      </Col>
    </Row>
    <Row className="px-5 py-3">
      <Col>
        <MessageContainer messages={messages} />
      </Col>
    </Row>
    <Row className="px-5 py-3">
      <Col>
        <SendMessageForm sendMessage={sendMessage} />
      </Col>
    </Row>
    <Row className="px-5 py-3">
      <Col>
        <Button
          variant="danger"
          onClick={leaveChatRoom}
          block
        >
          Leave Room
        </Button>
      </Col>
    </Row>
  </Container>
);

export default ChatRoom;
