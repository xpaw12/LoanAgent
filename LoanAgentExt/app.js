Ext.application({
    extend: 'LoanAgentExt.Application',

    name: 'LoanAgentExt',

    requires: [
        'LoanAgentExt.*'
    ],

    apiUrl: 'https://localhost:7158',

    launch: function () {
        Ext.Ajax.on('beforerequest', function (conn, options) {
            const token = localStorage.getItem('accessToken');
            if (token) {
                options.headers = options.headers || {};
                options.headers.Authorization = 'Bearer ' + token;
            }
        });

        const token = localStorage.getItem('accessToken');
        if (token && this.isTokenValid(token)) {
            const role = this.getUserRole(token); // Get the user's role
            if (role == 0) {
                this.setMainView('LoanAgentExt.view.main.Main'); // Regular user view
            } else if (role == 1) {
                LoanAgentExt.signalr.LoanHub.connect();
                this.setMainView('LoanAgentExt.view.admin.Admin'); // Admin view
            }
        } else {
            this.setMainView('LoanAgentExt.view.auth.Login');
            if (token) {
                localStorage.removeItem('accessToken');
            }
        }
    },

    isTokenValid: function (token) {
        if (!token) return false;

        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join('')
        );
        const { exp } = JSON.parse(jsonPayload);

        const currentTime = Math.floor(Date.now() / 1000);
        return exp && exp > currentTime;
    },

    getUserRole: function (token) {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64).split('').map(function (c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join('')
        );

        const payloadObject = JSON.parse(jsonPayload);
        return payloadObject.role; // Assuming your JWT contains the role field
    },

    getUserId: function () {
        const token = localStorage.getItem('accessToken');
        if (!token) return null;

        const payload = token.split('.')[1];
        if (!payload) return null;

        const decodedPayload = atob(payload);

        const payloadObject = JSON.parse(decodedPayload);

        return payloadObject.nameid || null;
    },

    setMainView: function (view) {
        if (this.mainView) {
            this.mainView.destroy();
        }
        this.mainView = Ext.create(view, {
            renderTo: Ext.getBody()
        });
    }
});