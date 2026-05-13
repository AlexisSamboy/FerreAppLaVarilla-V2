namespace FerreAppLaVarilla.UI.Services
{
    public static class AdminTheme
    {
        public static string EstilosGlobales => @"
            :root {
    --admin-blue: #0b1527;
    --admin-yellow: #f4b400;
    --bg-light: #f8fafc;
    --text-main: #0f172a;
    --text-muted: #64748b;
}

.admin-container {
    display: flex;
    background: var(--bg-light);
    min-height: 100vh;
    font-family: 'Segoe UI', sans-serif;
}

.admin-sidebar {
    width: 280px;
    background: var(--admin-blue);
    color: white;
    display: flex;
    flex-direction: column;
    padding: 30px 20px;
    position: sticky;
    top: 0;
    height: 100vh;
}

.sidebar-header {
    display: flex;
    align-items: center;
    gap: 12px;
    margin-bottom: 40px;
}

.logo-icon {
    background: var(--admin-yellow);
    color: var(--admin-blue);
    width: 45px;
    height: 45px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
    font-weight: bold;
}

.logo-text h2 {
    margin: 0;
    font-size: 20px;
    line-height: 1;
}

.logo-text span {
    color: var(--admin-yellow);
    font-size: 14px;
    text-transform: uppercase;
    letter-spacing: 1px;
}

.sidebar-nav {
    display: flex;
    flex-direction: column;
    gap: 8px;
    flex: 1;
}

.nav-link {
    color: #94a3b8;
    text-decoration: none;
    padding: 12px 15px;
    border-radius: 10px;
    display: flex;
    align-items: center;
    gap: 12px;
    transition: 0.2s;
    font-weight: 500;
}

    .nav-link:hover, .nav-link.active {
        background: rgba(255,255,255,0.05);
        color: white;
    }

    .nav-link.active {
        border-left: 4px solid var(--admin-yellow);
        background: rgba(244, 180, 0, 0.1);
        color: var(--admin-yellow);
    }

.sidebar-footer {
    border-top: 1px solid rgba(255,255,255,0.1);
    padding-top: 20px;
}

.admin-pill {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 15px;
}

.avatar {
    width: 40px;
    height: 40px;
    background: #3b82f6;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-weight: bold;
}

.admin-info strong {
    display: block;
    font-size: 14px;
}

.admin-info small {
    color: #64748b;
    font-size: 12px;
}

.btn-logout-minimal {
    background: transparent;
    border: 1px solid rgba(255,255,255,0.2);
    color: white;
    width: 100%;
    padding: 10px;
    border-radius: 8px;
    cursor: pointer;
    font-size: 13px;
}

.admin-main {
    flex: 1;
    padding: 40px;
    overflow-y: auto;
}

.admin-topbar {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 40px;
}

.welcome-text h1 {
    font-size: 28px;
    font-weight: 800;
    color: var(--text-main);
    margin: 0 0 5px 0;
}

.welcome-text p {
    color: var(--text-muted);
    margin: 0;
}

.topbar-actions {
    display: flex;
    gap: 20px;
    align-items: center;
}

.notification-badge {
    background: white;
    width: 45px;
    height: 45px;
    border-radius: 12px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 20px;
    position: relative;
    border: 1px solid #e2e8f0;
}

    .notification-badge span {
        position: absolute;
        top: -5px;
        right: -5px;
        background: #ef4444;
        color: white;
        font-size: 10px;
        padding: 2px 6px;
        border-radius: 50%;
    }

.date-display {
    background: white;
    padding: 10px 20px;
    border-radius: 12px;
    border: 1px solid #e2e8f0;
    font-weight: 600;
    color: var(--text-main);
    font-size: 14px;
}

.stats-cards-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
    gap: 25px;
    margin-bottom: 40px;
}

.card-stat {
    background: white;
    padding: 25px;
    border-radius: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    box-shadow: 0 10px 20px rgba(0,0,0,0.02);
    border: 1px solid #e2e8f0;
}

.card-body small {
    color: var(--text-muted);
    font-weight: 600;
    text-transform: uppercase;
    font-size: 11px;
    letter-spacing: 0.5px;
}

.card-body h2 {
    font-size: 24px;
    font-weight: 800;
    margin: 8px 0;
    color: var(--text-main);
}

.trend {
    font-size: 12px;
    font-weight: 700;
}

    .trend.positive {
        color: #10b981;
    }

.card-icon {
    width: 50px;
    height: 50px;
    border-radius: 14px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 24px;
}

    .card-icon.blue {
        background: #eff6ff;
        color: #3b82f6;
    }

    .card-icon.green {
        background: #f0fdf4;
        color: #22c55e;
    }

    .card-icon.orange {
        background: #fff7ed;
        color: #f97316;
    }

    .card-icon.purple {
        background: #faf5ff;
        color: #a855f7;
    }

.dashboard-lower-grid {
    display: grid;
    grid-template-columns: 2fr 1fr;
    gap: 25px;
}

.data-card {
    background: white;
    border-radius: 20px;
    padding: 30px;
    border: 1px solid #e2e8f0;
}

.card-header-flex {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 25px;
}

    .card-header-flex h3 {
        margin: 0;
        font-size: 18px;
        font-weight: 800;
    }

.btn-text {
    background: transparent;
    border: none;
    color: #3b82f6;
    font-weight: 700;
    cursor: pointer;
}

.admin-table {
    width: 100%;
    border-collapse: collapse;
}

    .admin-table th {
        text-align: left;
        padding: 12px;
        color: var(--text-muted);
        font-size: 12px;
        text-transform: uppercase;
        border-bottom: 1px solid #f1f5f9;
    }

    .admin-table td {
        padding: 15px 12px;
        font-size: 14px;
        border-bottom: 1px solid #f8fafc;
    }

.badge {
    padding: 5px 12px;
    border-radius: 8px;
    font-size: 11px;
    font-weight: 700;
    text-transform: uppercase;
}

    .badge.pendiente {
        background: #fef3c7;
        color: #d97706;
    }

    .badge.en-ruta {
        background: #e0f2fe;
        color: #0369a1;
    }

    .badge.entregado {
        background: #dcfce7;
        color: #15803d;
    }

.actions-card h3 {
    margin-bottom: 25px;
    font-size: 18px;
    font-weight: 800;
}

.actions-buttons-vertical {
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.btn-action {
    width: 100%;
    padding: 15px;
    border-radius: 12px;
    border: none;
    font-weight: 700;
    cursor: pointer;
    transition: 0.2s;
    text-align: left;
}

    .btn-action.primary {
        background: var(--admin-blue);
        color: white;
    }

    .btn-action.secondary {
        background: var(--admin-yellow);
        color: var(--admin-blue);
    }

    .btn-action.outline {
        background: white;
        border: 2px solid #e2e8f0;
        color: var(--text-main);
    }

.loader-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 50vh;
    color: var(--text-muted);
}

.custom-spinner {
    width: 50px;
    height: 50px;
    border: 5px solid #e2e8f0;
    border-top-color: var(--admin-blue);
    border-radius: 50%;
    animation: spin 1s linear infinite;
    margin-bottom: 15px;
}

@@keyframes spin {
    to {
        transform: rotate(360deg);
    }
}

        ";
    }
}