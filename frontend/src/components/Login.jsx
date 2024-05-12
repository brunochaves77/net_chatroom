import React, { useState } from "react";
import { Form, Button, Container, Row, Col } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

const Login = ({ login }) => {
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    await login(username, password);
    navigate("/");
  };

  return (
    <Container className="py-5">
      <Row className="justify-content-md-center">
        <Col md={6}>
          <Form
            onSubmit={handleSubmit}
            className="border p-4 rounded"
          >
            <h2 className="text-center mb-4">Login</h2>
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
                Login
              </Button>
            </div>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default Login;