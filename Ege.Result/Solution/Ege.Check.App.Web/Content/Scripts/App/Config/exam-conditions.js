module.exports = {
    basicMath: {
        minMark: 3
    },
    composition: {
        isPositive: function(mark) {
            if (mark && mark === 5) {
                return {
                    result: true,
                    string: 'зачёт'
                };
            }
            return {
                result: false,
                string: 'незачёт'
            };
        },
        minMark: 5
    }
};