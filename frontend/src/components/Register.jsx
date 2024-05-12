import React, { useState } from "react";
import { Form, Button, Container, Row, Col } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../auth";

const Register = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const success = await register(username, password);
      if (success) {
        alert("Registration successful!");
        navigate("/login");
      } else {
        alert("Failed to register.");
      }
    } catch (error) {
      alert("Failed to register:", error);
    }
  };

  return (
    <Container className="py-5">
      <Row className="justify-content-md-center">
        <Col md={6}>
          <Form
            onSubmit={handleSubmit}
            className="border p-4 rounded"
          >
            <h2 className="text-center mb-4">New Account</h2>
            <Form.Group
              controlId="formBasicUsername"
              className="mb-3"
            >
              <Form.Control
                type="text"
                placeholder="Enter username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </Form.Group>

            <Form.Group
              controlId="formBasicPassword"
              className="mb-3"
            >
              <Form.Control
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>

            <div className="text-center">
              <Button
                variant="primary"
                type="submit"
                className="w-75"
              >
                Sign up
              </Button>
            </div>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default Register;
