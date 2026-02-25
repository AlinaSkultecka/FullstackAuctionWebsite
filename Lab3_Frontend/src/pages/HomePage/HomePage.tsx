import { useNavigate } from "react-router-dom";
import "./HomePage.css";
import logo from "../../assets/logo.png";

export default function HomePage() {
  const navigate = useNavigate();

  return (
    <div className="home-container">
      <div className="home-content">
        
        {/* Big Logo */}
        <img src={logo} alt="BookBit Logo" className="home-logo" />

        {/* Optional tagline */}
        <h1 className="home-title">Welcome to BookBit</h1>
        <p className="home-subtitle">
          Discover books. Place bids. Win auctions.
        </p>

        {/* Login Button */}
        <button
          className="home-login-btn"
          onClick={() => navigate("/login")}
        >
          Login or Sign Up        </button>

      </div>
    </div>
  );
}