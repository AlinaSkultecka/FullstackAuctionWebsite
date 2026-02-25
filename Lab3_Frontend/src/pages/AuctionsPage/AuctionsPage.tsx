import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import "./AuctionsPage.css";
import logo from "../../assets/logo.png"; // Make sure to have a logo image in this path or adjust accordingly  

type Auction = {
  id: number;
  title: string;
  author?: string;
  imageUrl?: string;
  currentPrice: number;
  endsAt: string; // ISO string
};

export default function AuctionsPage() {
  const navigate = useNavigate();

  // Replace later with API data from your backend
  const [auctions] = useState<Auction[]>([
    {
      id: 1,
      title: "Clean Code",
      author: "Robert C. Martin",
      imageUrl: "",
      currentPrice: 120,
      endsAt: "2026-03-01T18:00:00Z",
    },
    {
      id: 2,
      title: "The Pragmatic Programmer",
      author: "Andrew Hunt",
      imageUrl: "",
      currentPrice: 90,
      endsAt: "2026-03-03T12:00:00Z",
    },
    {
      id: 3,
      title: "Design Patterns",
      author: "Gang of Four",
      imageUrl: "",
      currentPrice: 90,
      endsAt: "2026-03-03T12:00:00Z",
    }
  ]);

  const [query, setQuery] = useState("");

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return auctions;
    return auctions.filter(
      (a) =>
        a.title.toLowerCase().includes(q) ||
        (a.author ?? "").toLowerCase().includes(q)
    );
  }, [auctions, query]);

  const goToDetails = (id: number) => {
    navigate(`/auctions/${id}`); // you’ll create AuctionDetailsPage for this
  };

  return (
    <div className="auctions-container">
       <header className="topbar">
          {/* LEFT: logo */}
          <div className="topbar-left">
            <img className="topbar-logo" src={logo} alt="BookBit" />
          </div>

          {/* MIDDLE: search */}
          <form
            className="topbar-search"
            onSubmit={(e) => {
              e.preventDefault();
              // You already filter auctions live by query,
              // so submit can just keep focus or do nothing.
            }}
          >
            <input
              className="topbar-search-input"
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              placeholder="What are you looking for..."
            />
            <button className="topbar-search-btn" type="submit">
              Search
            </button>
          </form>

          {/* RIGHT: account */}
          <div className="topbar-right">
            <button
              className="account-btn"
              type="button"
              onClick={() => navigate("/account")}
              aria-label="My account"
              title="My account"
            >
              <span className="account-icon">👤</span>
              <span className="account-text">Account</span>
            </button>
          </div>
        </header>

      <main className="auctions-main">
        {filtered.length === 0 ? (
          <div className="empty-state">No auctions match your search.</div>
        ) : (
          <div className="auctions-grid">
            {filtered.map((a) => (
              <button
                key={a.id}
                className="auction-card"
                onClick={() => goToDetails(a.id)}
                type="button"
              >
                <div className="auction-image">
                  {a.imageUrl ? (
                    <img src={a.imageUrl} alt={a.title} />
                  ) : (
                    <div className="img-placeholder">📚</div>
                  )}
                </div>

                <div className="auction-info">
                  <h3 className="auction-title">{a.title}</h3>
                  <p className="auction-author">{a.author ?? "Unknown author"}</p>

                  <div className="auction-meta">
                    <span className="price-pill">Current: {a.currentPrice} kr</span>
                    <span className="end-pill">
                      Ends: {new Date(a.endsAt).toLocaleDateString()}
                    </span>
                  </div>

                  <div className="cta-row">
                    <span className="cta">Open & bid →</span>
                  </div>
                </div>
              </button>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}