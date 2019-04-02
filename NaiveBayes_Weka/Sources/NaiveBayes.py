from scipy.io import arff
import random
import math


def loadArff(filename):
    dataset, meta = arff.loadarff(filename)
    dataset2 = []
    for i in range(len(dataset)):
        new = []
        for j in range(len(dataset[i])):
            if j != 13:
                new.append(dataset[i][j].decode('utf-8'))
            else:
                new.append(int(dataset[i][j]))
        dataset2.append(new)
    return dataset2


def splitDataset(dataset, splitRatio):
    trainSize = int(len(dataset) * splitRatio)
    trainSet = []
    copy = list(dataset)
    while len(trainSet) < trainSize:
        index = random.randrange(len(copy))
        trainSet.append(copy.pop(index))
    return [trainSet, copy]


def dem(trainingSet):
    cnt = {}
    # init all element in dictionary cnt = 0
    for i in range(len(trainingSet)):
        for j in range(len(trainingSet[i])-1):  # -label
            cnt[(j, trainingSet[i][j], trainingSet[i][len(trainingSet[i])-1])] = 0
    for i in range(len(trainingSet)):
        for j in range(len(trainingSet[i])-1):  # -label
            cnt[(j, trainingSet[i][j], trainingSet[i][len(trainingSet[i])-1])] += 1
    return cnt


def prob(trainingSet):
    cnt = dem(trainingSet)
    cntLabel = {}
    for i in range(len(trainingSet)):
        cntLabel[trainingSet[i][len(trainingSet[i])-1]] = 0

    for i in range(len(trainingSet)):
        cntLabel[trainingSet[i][len(trainingSet[i])-1]] += 1
    prob = {}
    for key, value in cnt.items():
        prob[key] = value/cntLabel[key[2]]
    for key, value in cntLabel.items():
        prob[key] = value/len(trainingSet)
    return prob, cntLabel


def predict(trainingSet, query):  # probTable: bảng xác suất đã tính, query: câu truy vấn
    labels = ['mammal', 'bird', 'reptile', 'fish',
              'amphibian', 'insect', 'invertebrate']
    probTable, cntLabel = prob(trainingSet)
    maxP = 0
    bestLabel = ''
    for i in range(len(labels)):
        product = 1
        for j in range(len(query)-1):  # attribute from 0 to 16
            if (j, query[j], labels[i]) not in probTable:
                probTable[(j, query[j], labels[i])] = 1 / \
                    (cntLabel[labels[i]] + len(trainingSet))
            if query[j] != '?':
                product *= probTable[(j, query[j], labels[i])]
        if product >= maxP:
            maxP = product
            bestLabel = labels[i]
    return bestLabel


def getPredictions(trainingSet, testSet):  # cntLabel: đếm số lượng các label
    predictions = []
    for i in range(len(testSet)):
        result = predict(trainingSet, testSet[i])
        predictions.append(result)
    return predictions


def getAccuracy(testSet, predictions):
    correct = 0
    for i in range(len(testSet)):
        if testSet[i][-1] == predictions[i]:
            correct += 1
    return (correct/float(len(testSet))) * 100.0


def main():
    dataset = loadArff('zoo.arff')
    splitRatio = 1.0
    trainingSet, testSet = splitDataset(dataset, splitRatio)
    testSet = loadArff('test_data_set.arff')
    predictions = getPredictions(trainingSet, testSet)
    accuracy = getAccuracy(testSet, predictions)
    print('Tỷ lệ chia trainingSet và testset: ', splitRatio)
    print('Tổng số lượng mẫu trong zoo.arff', len(trainingSet))
    print('Số mẫu trong tập dữ liệu test', len(testSet))
    print('Tập dữ liệu cần gắn nhãn')
    for _ in range(len(testSet)):
        print(testSet[_])

    print('Kết quả dự đoán')
    print(predictions)

    # print('Accuracy = ',accuracy,'%')
main()
