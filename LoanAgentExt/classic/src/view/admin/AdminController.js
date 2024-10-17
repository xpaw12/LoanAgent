Ext.define('LoanAgentExt.view.admin.AdminController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.admin',
    init: function (view) {
        this.loadAdminLoans();
    },

    onItemSelected: function (sender, record) {
        Ext.Msg.confirm('Confirm', 'Are you sure?', 'onConfirm', this);
    },

    onConfirm: function (choice) {
        if (choice === 'yes') {
            //
        }
    },

    loadAdminLoans: function () {
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/admin',
            method: 'GET',
            success: function (response) {
                const loansData = Ext.decode(response.responseText);

                const store = Ext.getStore('adminloans');
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
