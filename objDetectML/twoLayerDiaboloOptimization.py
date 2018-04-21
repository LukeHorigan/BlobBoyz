import os
import sys
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt

from sklearn.preprocessing import StandardScaler
from sklearn.model_selection import train_test_split

import tensorflow as tf

## Main algorithm and helper functions:
def optimizeDiabolo(my_train=None, my_test=None,
                    n_iter=100, batch_size=5, learning_rate=0.001, seedNum=42,
                    number_of_neurons_first_layer=4,number_of_neurons_second_layer=4):
    '''
    Implementation of a two-layer Diabolo Neural Network
    __originalAuthor__: L. Merkel (Apache2.0 License)
    __Reference__: H. Schwenk 1998
    __Maintainer__: D. Chen
    '''
    number_of_features = len(my_train[0])
    ## Initialize parameters:
    We1 = tf.Variable(tf.random_normal([number_of_features, number_of_neurons_first_layer], dtype=tf.float32))
    be1 = tf.Variable(tf.zeros([number_of_neurons_first_layer]))

    We2 = tf.Variable(tf.random_normal([number_of_neurons_first_layer, number_of_neurons_second_layer], dtype=tf.float32))
    be2 = tf.Variable(tf.zeros([number_of_neurons_second_layer]))

    Wd1 = tf.Variable(tf.random_normal([number_of_neurons_second_layer, number_of_neurons_first_layer], dtype=tf.float32))
    bd1 = tf.Variable(tf.zeros([number_of_neurons_first_layer]))

    Wd2 = tf.Variable(tf.random_normal([number_of_neurons_first_layer, number_of_features], dtype=tf.float32))
    bd2 = tf.Variable(tf.zeros([number_of_features]))

    X = tf.placeholder(dtype=tf.float32, shape=[None, number_of_features])
    encoding = tf.nn.tanh(tf.matmul(X, We1) + be1)
    encoding = tf.matmul(encoding, We2) + be2
    decoding = tf.nn.tanh(tf.matmul(encoding, Wd1) + bd1)
    decoded = tf.matmul(decoding, Wd2) + bd2

    ## Define Diabolo loss:
    loss = tf.sqrt(tf.reduce_mean(tf.square(tf.subtract(X, decoded))))
    train_step = tf.train.RMSPropOptimizer(learning_rate).minimize(loss)

    ## Iterative optimization:
    tf.set_random_seed(seedNum)
    sess = tf.Session()
    sess.run(tf.global_variables_initializer())
    loss_train, loss_test = [], [];
    for i in range(n_iter):
        if batch_size > 0:
            for j in range(np.shape(my_train)[0] // batch_size):
                batch_data = get_batch(my_train, j, batch_size) #custom function
                sess.run(train_step, feed_dict={X: batch_data})
        else:
            sess.run(train_step, feed_dict={X: my_train})

        lt = sess.run(loss, feed_dict={X: my_train})
        lv = sess.run(loss, feed_dict={X: my_test})
        loss_train.append(lt)
        loss_test.append(lv)
        if i % 5==0 or i==n_iter-1:
            print('Now on iteration {0}: Training loss = {1:.2f}, Validation loss = {2:.2f}'.format(i, lt, lv))

    return loss_train, loss_test, X, encoding, decoded;

def get_batch(data, i, size):
    '''
    Helper function for Stochastic gradient descent
    __author__: L. Merkel (Apache2.0 License)
    '''
    return data[range(i*size, (i+1)*size)]; 

def timeEncoding(df):
    '''
    Helper function to convert the time information column into two new columns
    Specific to dataset of interest
    __author__: L. Merkel (Apache2.0 License)
    '''
    df['click_time'] = pd.to_datetime(df['click_time'])

    df['hour'] = df['click_time'].dt.hour
    df['weekday'] = df['click_time'].dt.weekday

    df.drop('click_time', axis=1, inplace=True)
    return df;

def distribute_in_range(data, min=0, max=1):
    '''
    Generate prediction for distribute the loss from 0 and 1
        data - test data
        min, max - output constraints
    __originalAuthor__: L. Merkel (Apache2.0 License)
    __Reference__: H. Schwenk 1998
    __Maintainer__: D. Chen
    '''
    max_data = np.max(data);
    a = (max - min) / (max_data - np.min(data));
    b = max - a * max_data;
    res = a * data + b;
    return res;
