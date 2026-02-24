import { useState } from "react";
import "./LoginPage.css";
import logo from "../assets/logo.png";

/*
  LoginPage component
  Handles both Login and Sign Up modes
*/
const LoginPage = () => {

  // Controls whether we are in Login mode (true) or Sign Up mode (false)
  const [isLogin, setIsLogin] = useState(true);

  // Stores username input value
  const [username, setUsername] = useState("");

  // Stores email input value (only used during Sign Up)
  const [email, setEmail] = useState("");

  // Stores password input value
  const [password, setPassword] = useState("");

  /*
    Handles form submission.
    Prevents page refresh and logs different data
    depending on Login or Register mode.
  */
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault(); // Prevent page reload

    if (isLogin) {
      // If Login mode → only username + password
      console.log("Login:", username, password);
    } else {
      // If Register mode → username + email + password
      console.log("Register:", username, email, password);
    }
  };

  return (
    // Main container that centers the card
    <div className="auth-container">

      {/* Card wrapper (split layout) */}
      <div className="auth-card">

        {/* ================= LEFT SIDE ================= */}
        <div className="auth-left">

          {/* Website Logo */}
          <img src={logo} alt="BookBid Logo" className="auth-logo" />

          {/* Greeting Text */}
          <p>Welcome to the smartest book auction platform.</p>
        </div>


        {/* ================= RIGHT SIDE ================= */}
        <div className="auth-right">

          {/* Title changes based on mode */}
          <h2>{isLogin ? "Login" : "Sign Up"}</h2>

          {/* Form submission handled by handleSubmit */}
          <form onSubmit={handleSubmit}>

            {/* 
              Email input is ONLY shown when user is signing up.
              Conditional rendering using !isLogin
            */}
            {!isLogin && (
              <input
                type="email"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            )}

            {/* Username input */}
            <input
              type="text"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />

            {/* Password input */}
            <input
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            {/* Button text changes depending on mode */}
            <button type="submit">
              {isLogin ? "Login" : "Create Account"}
            </button>

          </form>

          {/* Switch between Login and Sign Up */}
          <div className="switch-mode">

            {/* Text changes depending on mode */}
            {isLogin 
              ? "Don't have an account?" 
              : "Already have an account?"
            }

            {/* 
              Clicking this toggles the mode 
              and re-renders the component
            */}
            <span onClick={() => setIsLogin(!isLogin)}>
              {isLogin ? " Sign up" : " Login"}
            </span>

          </div>
        </div>

      </div>
    </div>
  );
};

export default LoginPage;