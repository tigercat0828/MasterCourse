# Introduction
### Type of AI
- Traditional AI : manually adding explicit rule to follow
- Machine Learning : automatically learning rule from a large set of examples.
- Deep Learning : make machine smart using parallel simple algorirhms to extract rule from a large number of example.

### Why is Machine Learning
- Associations : expressed as conditional probabilities $P(Y|X)$
![[Pasted image 20230920200415.png]]
- Regularity : expressed as functions $y=f(x)$ or $y=f(x|\theta)$
	- Linear Model : $y=a_1x+a_0, \theta=(a_0, a_1)$
	- Quadratic Model : $y=a_2x^2+a_1x+a_0, \theta=(a_0, a_1, a_2)$
	- Gaussian Model : $y=\frac{1}{\sqrt{2\pi\sigma}}exp(\frac{(x-\mu)^2}{2\sigma^2}), \theta=(\mu, \sigma)^T$
- Structure
	- Clustering
	- Density estimation
	- Image segmentation
- Knowledge
	- scoring (by some attrib) 
	- pattern recognition(face, speech, OCR...)

### Fundamental Ingredients
Training, validation, test examples
- Hypothesis set : 
- Inductive bias, Prior knowledge :
- Learning algorithm :
### Type of ML
- Supervised learning
- Unsupervised learning
- Semi-supervised learning
- Self-supervised Learning
- Temporally Supervised Learning
- Online Learning
- Active Learning
- Reinforce Learning