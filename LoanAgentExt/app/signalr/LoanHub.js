Ext.define('LoanAgentExt.signalr.LoanHub', {
    singleton: true,

    connection: null,

    connect: function () {
        const me = this;

        me.connection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7158/loanHub')
            .configureLogging(signalR.LogLevel.Information)
            .build();

        me.connection.start().then(function () {
            console.log('SignalR Connected to LoanHub.');
        }).catch(function (err) {
            console.error('SignalR Connection failed: ', err);
        });

        me.connection.on('ReceiveLoanUpdate', function (loan) {
            me.onLoanReceived(loan);
        });
    },

    onLoanReceived: function (loan) {
        console.log('New Loan Received: ', loan);

        const grid = Ext.ComponentQuery.query('adminloansgrid')[0];
        if (grid) {
            const store = grid.getStore();
            store.add(loan);
        }
    }
});
