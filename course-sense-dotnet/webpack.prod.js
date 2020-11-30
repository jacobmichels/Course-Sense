const {merge} = require('webpack-merge');
const common = require('./webpack.common.js');
const BrotliPlugin = require('brotli-webpack-plugin');

module.exports = merge(common,{
    mode:'production',
    plugins:[
        new BrotliPlugin({
            asset: '[path].br[query]',
            test: /\.(js|css|html|svg)$/,
        })
    ]
});