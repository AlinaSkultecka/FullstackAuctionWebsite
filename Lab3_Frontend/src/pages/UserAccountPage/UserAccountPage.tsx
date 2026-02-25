import { useMemo, useState } from "react";
import "./UserAccountPage.css";
import logo from "../../assets/logo.png";
import { Link } from "react-router-dom";

type User = {
  id: number;
  userName: string;
  email: string;
  photoUrl?: string; // optional if you support it
};

export default function UserAccountPage() {
  // Replace with your Context user later
  const [user, setUser] = useState<User>({
    id: 1,
    userName: "Alina",
    email: "alina@email.com",
    photoUrl: "",
  });

  const [preview, setPreview] = useState<string>(user.photoUrl ?? "");

  // Change email (optional)
  const [email, setEmail] = useState(user.email);

  // Change password
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [repeatNewPassword, setRepeatNewPassword] = useState("");

  const passwordValid = useMemo(() => {
    if (!newPassword) return true;
    return newPassword.length >= 6 && newPassword === repeatNewPassword;
  }, [newPassword, repeatNewPassword]);

  const handlePickPhoto = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    const url = URL.createObjectURL(file);
    setPreview(url);

    // Later: upload to backend, then set user.photoUrl from response
    // setUser(prev => ({ ...prev, photoUrl: url }))
  };

  const handleSaveProfile = async (e: React.FormEvent) => {
    e.preventDefault();

    // Example: call API PUT /users/me (or /users/{id}) to update email/photo
    // For VG: keep userName readonly and do NOT send it.
    setUser((prev) => ({ ...prev, email }));
    alert("Profile saved!");
  };

  const handleChangePassword = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!passwordValid) {
      alert("New password must be at least 6 chars and match repeat.");
      return;
    }

    // Example: call API PUT /users/me/password (currentPassword, newPassword)
    // Clear fields after success:
    setCurrentPassword("");
    setNewPassword("");
    setRepeatNewPassword("");
    alert("Password updated!");
  };

  return (
    <div className="account-container">

         {/* Logo above card */}
        <div className="account-logo-wrapper">
        <Link to="/">
            <img src={logo} alt="BookBit Logo" className="account-logo" />
        </Link>
        </div>

      <div className="account-card">
        {/* LEFT */}
        <div className="account-left">
          <p>My Account</p>

          <div className="profile-photo">
            {preview ? (
              <img src={preview} alt="Profile" />
            ) : (
              <div className="profile-placeholder">👤</div>
            )}
          </div>

          <label className="photo-btn">
            Change photo
            <input type="file" accept="image/*" onChange={handlePickPhoto} />
          </label>

          <div className="user-mini">
            <div className="pillUser">Username: <b>{user.userName}</b></div>
            <div className="pillUser">Email: <b>{user.email}</b></div>
          </div>

          <div className="section actions">
            <button className="secondary" type="button" onClick={() => alert("Logout")}>
              Logout
            </button>
            <button className="danger" type="button" onClick={() => alert("Delete account")}>
              Delete account
            </button>
          </div>
        </div>

        {/* RIGHT */}
        <div className="account-right">
          <h2>Account settings</h2>

          <div className="section">
            <h3>Profile</h3>
            <form onSubmit={handleSaveProfile} className="form-grid">
              <div className="field span-2">
                <label>Email</label>
                <input
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    type="email"
                />
                </div>

              <button className="primary" type="submit">
                Save email & photo
              </button>
            </form>
            </div>
            
            <form onSubmit={handleChangePassword} className="pw-form">
            {/* Row 1 */}
            <div className="field">
                <label>Current password</label>
                <input
                type="password"
                value={currentPassword}
                onChange={(e) => setCurrentPassword(e.target.value)}
                placeholder="Enter current password"
                required
                />
            </div>

            {/* Row 2 (two columns) */}
            <div className="pw-row">
                <div className="field">
                <label>New password</label>
                <input
                    type="password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    placeholder="New password"
                    required
                />
                </div>

                <div className="field">
                <label>Repeat new password</label>
                <input
                    type="password"
                    value={repeatNewPassword}
                    onChange={(e) => setRepeatNewPassword(e.target.value)}
                    placeholder="Repeat new password"
                    required
                />
                </div>
            </div>

            {!passwordValid && (
                <small className="error">Passwords must match (and be 6+ chars).</small>
            )}

            <button className="primary" type="submit">
                Update password
            </button>
            </form>
        </div>
      </div>
    </div>
  );
}