## Helper algorithm/module: CNN Model for Blob Detection

# In[1]:
## Load python libraries and define main function
## Download appropriate methods if needed
import os
import numpy as np
from __future__ import print_function
import keras
from keras.layers import Activation, Conv2D, Dense, Dropout, Flatten, MaxPooling2D
from keras.models import Sequential
from keras.preprocessing.image import ImageDataGenerator

def model_init(x_train, dropOutRate=0.25,
               opt=keras.optimizers.rmsprop(lr=1e-4, decay=1e-6)):
    model = Sequential();
    model.add(Conv2D(32, (3, 3), padding='same', input_shape=x_train.shape[1:]));
    model.add(Activation('relu'));
    model.add(Conv2D(32, (3, 3)));
    model.add(Activation('relu'));
    model.add(MaxPooling2D(pool_size=(2,2)));
    model.add(Dropout(dropOutRate))

    model.add(Conv2D(64, (3,3), padding='same'))
    model.add(Activation('relu'))
    model.add(Conv2D(64, (3,3)))
    model.add(Activation('relu'))
    model.add(MaxPooling2D(pool_size=(2,2)))
    model.add(Dropout(dropOutRate))

    model.add(Flatten())
    model.add(Dense(512))
    model.add(Activation('relu'))
    model.add(Dropout(2*dropOutRate))
    model.add(Dense(10))
    model.add(Activation('sigmoid')) #or softmax

    model.compile(loss='binary_crossentropy',optimizer=opt,metrics=['accuracy','mae'])

    return model;


# In[2]:
## Load toy dataset:
# from keras.datasets import cifar10 #toy data set
# (x_train, y_train), (x_test, y_test) = cifar10.load_data();
# print('x_train shape:', x_train.shape);
# print(x_train.shape[0], 'train samples');
# print(x_test.shape[0], 'test samples');
# y_train = keras.utils.to_categorical(y_train, 10);
# y_test = keras.utils.to_categorical(y_test, 10);
# print(y_train[0:5])
# print(y_test[0:5])

##...or our own bitmapdata set:

## Further processing of data:
x_train = x_train.astype('float32');
x_test = x_test.astype('float32');

## Random selection to match reality
random_sample = np.random.choice(a=range(0, x_train.shape[0]), size=300, replace=False);
x_train = x_train[random_sample];
y_train = y_train[random_sample];

random_sample = np.random.choice(a=range(0, x_train.shape[0]), size=100, replace=False)
x_test = x_test[random_sample];
y_test = y_test[random_sample];

x_train /= 255;
x_test /= 255;


# In[3]:
## Define (hyper)-parameters:
batchSize = 100;
epochs = 20; #early stopping
num_predictions = 20;
data_augmentation = True;

## Fit model:
model = model_init(x_train);

## Data augmentation (highly recommended) followed by model fitting
if not data_augmentation:
    print('Fitting model without data augmentation...');
    model.fit(x_train, y_train,
              batch_size=batchSize,
              epochs=epochs,
              validation_data=(x_test, y_test),
              shuffle=True);
else:
    print('Proceed to data augmentation...')
    # This will do preprocessing and realtime data augmentation:
    datagen = ImageDataGenerator(
        featurewise_std_normalization=True, samplewise_std_normalization=True,
        featurewise_center=False, samplewise_center=False,
        width_shift_range=0.1, height_shift_range=0.1,
        horizontal_flip=False, vertical_flip=False,
        zca_whitening=False,
        rotation_range=0);
    datagen.fit(x_train)
    print("Data-augmentation fit complete!")

    # Fit the model on the batches generated by datagen.flow().
    model.fit_generator(datagen.flow(x_train, y_train,batch_size=batchSize),
                        epochs=epochs,
                        validation_data=(x_test, y_test),
                        shuffle=True,
                        workers=4)

# In[4]:
## Evaluate final model:
scores = model.evaluate(x_test, y_test, verbose=2);
print('Test accuracy:', scores[1]);

## Output results if baseline threshold is met
if scores[1] > 0.8:
    predictions_train = model.predict_classes(x_train)
    predictions_test = model.predict_classes(x_test)
    predictions = np.append(predictions_train, predictions_test);
    res = np.where(predictions == 1); 
    np.savetxt("/Users/DavidKevinChen/Downloads/testResults.txt", res, delimiter=" ", fmt="%s");
    print("Results exported!")
else:
    raise ValueError("Accuracy is too low! Please re-tune hyperparameter!")
