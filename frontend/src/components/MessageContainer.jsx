import React from "react";
import { Container } from "react-bootstrap";

const MessageContainer = ({ messages }) => {
  const lastMessages = messages.slice(-50);

  return (
    <Container className="border p-3 rounded">
      {lastMessages.map((msg, index) => (
        <div
          key={index}
          className="mb-2"
        >
          <span className="fw-bold">{msg.name}:</span> {msg.msg}
        </div>
      ))}
    </Container>
  );
};

export default MessageContainer;
