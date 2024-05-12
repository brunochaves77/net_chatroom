import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";

const SendMessageForm = ({ sendMessage }) => {
  const [msg, setMessage] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    if (msg.trim() !== "") {
      sendMessage(msg);
      setMessage("");
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
      <InputGroup className="mb-3">
        <Form.Control
          placeholder="Type your message here..."
          value={msg}
          onChange={(e) => setMessage(e.target.value)}
        />
        <Button
          variant="primary"
          type="submit"
        >
          Send
        </Button>
      </InputGroup>
    </Form>
  );
};

export default SendMessageForm;
