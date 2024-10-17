Ext.define('LoanAgentExt.view.admin.LoansGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'adminloansgrid',

    title: 'Admin panel',

    store: {
        type: 'adminloans'
    },

    columns: [
        { text: 'Loan Owner', dataIndex: 'loanOwnerName', flex: 1 },
        { text: 'Loan Amount', dataIndex: 'loanAmount', flex: 1 },
        { text: 'Currency', dataIndex: 'currency', flex: 1 },
        { text: 'Loan Type', dataIndex: 'loanType', flex: 1 },
        { text: 'Loan State', dataIndex: 'loanState', flex: 1 },
        { text: 'Start Date', dataIndex: 'startDate', flex: 1 },
        { text: 'End Date', dataIndex: 'endDate', flex: 1 },
        { text: 'Created Date', dataIndex: 'createdDateTime', flex: 1 },
        {
            xtype: 'actioncolumn',
            text: 'Actions',
            width: 100,
            items: [{
                iconCls: 'fa fa-check custom-action-icon',
                tooltip: 'Approve Loan',
                handler: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanId = record.get('loanId');
                    const approveStatus = 'Approved';
                    Ext.Msg.confirm('Confirm Submission', 'Are you sure you want to approve this loan?', function (btn) {
                        if (btn === 'yes') {
                            grid.up('adminloansgrid').changeLoanStatus(loanId, approveStatus);
                        }
                    });
                },
                isActionDisabled: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanState = record.get('loanState');

                    return loanState === 'Approved' || loanState === 'Declined';
                },
                scope: this
            }, {
                iconCls: 'fas fa-times custom-action-icon',
                tooltip: 'Decline Loan',
                handler: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanId = record.get('loanId');
                    const approveStatus = 'Declined';
                    Ext.Msg.confirm('Confirm Submission', 'Are you sure you want to decline this loan?', function (btn) {
                        if (btn === 'yes') {
                            grid.up('adminloansgrid').changeLoanStatus(loanId, approveStatus);
                        }
                    });
                },
                isActionDisabled: function (grid, rowIndex, colIndex) {
                    const record = grid.getStore().getAt(rowIndex);
                    const loanState = record.get('loanState');

                    return loanState === 'Approved' || loanState === 'Declined';
                },
                scope: this
            }]
        }
    ],

    listeners: {
        itemclick: function (view, record) {
        }
    },

    changeLoanStatus: function (loanId, status) {
        Ext.Ajax.request({
            url: LoanAgentExt.app.apiUrl + '/api/loans/change-status',
            method: 'PUT',
            jsonData: {
                loanId: loanId,
                newLoanState: status
            },
            success: function (response) {
                Ext.Msg.alert('Success', 'Loan status changed successfully!', function () {
                    window.location.reload();
                },);
            },
            failure: function () {
                Ext.Msg.alert('Error', 'Failed to update loan status.');
            },
            scope: this
        });
    }
});
