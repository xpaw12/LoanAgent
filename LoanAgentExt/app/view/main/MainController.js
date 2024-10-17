Ext.define('LoanAgentExt.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.main',

    init: function (view) {
        this.loadUserLoans();
    },

    onItemSelected: function (sender, record) {
        Ext.Msg.confirm('Confirm', 'Are you sure?', 'onConfirm', this);
    },

    onConfirm: function (choice) {
        if (choice === 'yes') {
            //
        }
    },

    loadUserLoans: function () {
        const userId = LoanAgentExt.app.getUserId();
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/' + (userId ? '?userId=' + userId : ''),
            method: 'GET',
            success: function (response) {
                const loansData = Ext.decode(response.responseText);

                const store = Ext.getStore('userloans');
                if (store) {
                    store.loadData(loansData);
                } else {
                    console.log("Store not found.");
                }
            },
            failure: function () {
                Ext.Msg.alert('Error', 'Failed to load user loans.');
            }
        });
    }
});
