import { useMemo, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./AuctionDetailsPage.css";
import logo from "../../assets/logo.png";

type Bid = {
  id: number;
  amount: number;
  bidderName: string;
  createdAt: string; // ISO
};

type AuctionDetails = {
  id: number;
  title: string;
  author?: string;
  description?: string;
  imageUrl?: string;
  endsAt: string; // ISO
  bids: Bid[];
};

export default function AuctionDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  // Mock “DB” (replace later with API fetch by id)
  const auction: AuctionDetails | undefined = useMemo(() => {
    const all: AuctionDetails[] = [
      {
        id: 1,
        title: "Clean Code",
        author: "Robert C. Martin",
        description:
          "A handbook of agile software craftsmanship. Learn how to write clean, readable, maintainable code.",
        imageUrl: "",
        endsAt: "2026-03-01T18:00:00Z",
        bids: [
          { id: 1, amount: 80, bidderName: "Lisa", createdAt: "2026-02-20T10:10:00Z" },
          { id: 2, amount: 120, bidderName: "Mark", createdAt: "2026-02-21T15:30:00Z" },
        ],
      },
      {
        id: 2,
        title: "The Pragmatic Programmer",
        author: "Andrew Hunt",
        description:
          "Classic book about pragmatic thinking, coding practices, and becoming a better developer.",
        imageUrl: "",
        endsAt: "2026-03-03T12:00:00Z",
        bids: [{ id: 1, amount: 90, bidderName: "Nora", createdAt: "2026-02-22T09:05:00Z" }],
      },
      {
        id: 3,
        title: "Design Patterns",
        author: "Gang of Four",
        description:
          "Foundational patterns for reusable object-oriented software. Great for OOP and architecture.",
        imageUrl: "",
        endsAt: "2026-03-03T12:00:00Z",
        bids: [],
      },
    ];

    const numericId = Number(id);
    return all.find((a) => a.id === numericId);
  }, [id]);

  const [bids, setBids] = useState<Bid[]>(auction?.bids ?? []);
  const [bidAmount, setBidAmount] = useState<string>("");
  const [error, setError] = useState<string>("");

  const currentHighest = useMemo(() => {
    if (bids.length === 0) return 0;
    return Math.max(...bids.map((b) => b.amount));
  }, [bids]);

  if (!auction) {
    return (
      <div className="details-container">
        <div className="details-card">
          <div className="details-topbar">
            <img className="details-logo" src={logo} alt="BookBit" />
            <button className="details-back" onClick={() => navigate("/auctions")}>
              ← Back
            </button>
          </div>

          <div className="details-empty">
            Auction not found.
            <button className="details-primary" onClick={() => navigate("/auctions")}>
              Go to auctions
            </button>
          </div>
        </div>
      </div>
    );
  }

  const handlePlaceBid = (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    const amount = Number(bidAmount);

    if (!bidAmount.trim() || Number.isNaN(amount)) {
      setError("Please enter a valid number.");
      return;
    }

    if (amount <= currentHighest) {
      setError(`Your bid must be higher than ${currentHighest} kr.`);
      return;
    }

    // Later: POST to backend. For now: update local state.
    const newBid: Bid = {
      id: Date.now(),
      amount,
      bidderName: "You",
      createdAt: new Date().toISOString(),
    };

    setBids((prev) => [newBid, ...prev]);
    setBidAmount("");
  };

  return (
    <div className="details-container">
      <div className="details-card">
        {/* Top bar */}
        <div className="details-topbar">
          <img className="details-logo" src={logo} alt="BookBit" />

          <div className="details-top-actions">
            <button className="details-back" onClick={() => navigate("/auctions")}>
              ← Back to auctions
            </button>
            <button className="details-account" onClick={() => navigate("/account")}>
              👤 Account
            </button>
          </div>
        </div>

        {/* Main layout */}
        <div className="details-grid">
          {/* LEFT: book */}
          <div className="book-card">
            <div className="book-image">
              {auction.imageUrl ? (
                <img src={auction.imageUrl} alt={auction.title} />
              ) : (
                <div className="book-placeholder">📚</div>
              )}
            </div>

            <div className="book-info">
              <h1 className="book-title">{auction.title}</h1>
              <p className="book-author">{auction.author ?? "Unknown author"}</p>

              <div className="book-meta">
                <span className="pill">Ends: {new Date(auction.endsAt).toLocaleString()}</span>
                <span className="pill">Current highest: {currentHighest} kr</span>
              </div>

              <p className="book-desc">
                {auction.description ?? "No description available."}
              </p>
            </div>
          </div>

          {/* RIGHT: bid */}
          <div className="bid-card">
            <h2 className="bid-title">Place a bid</h2>

            <form className="bid-form" onSubmit={handlePlaceBid}>
              <label className="bid-label">Your bid (kr)</label>

              <div className="bid-row">
                <input
                  className="bid-input"
                  value={bidAmount}
                  onChange={(e) => setBidAmount(e.target.value)}
                  placeholder={`More than ${currentHighest}`}
                  inputMode="numeric"
                />
                <button className="details-primary" type="submit">
                  Bid
                </button>
              </div>

              {error && <div className="bid-error">{error}</div>}

              <div className="bid-hint">
                Tip: Your bid must be higher than the current highest bid.
              </div>
            </form>

            <h3 className="history-title">Bid history</h3>

            {bids.length === 0 ? (
              <div className="history-empty">No bids yet. Be the first!</div>
            ) : (
              <div className="history-list">
                {bids.map((b) => (
                  <div className="history-item" key={b.id}>
                    <div className="history-left">
                      <div className="history-amount">{b.amount} kr</div>
                      <div className="history-name">{b.bidderName}</div>
                    </div>
                    <div className="history-date">
                      {new Date(b.createdAt).toLocaleString()}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}