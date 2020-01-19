(defblock :name clone :is-top t
	(worker
		:worker-name clone
		:verb "clone"
		:doc "Clone a repo."
		:usage-samples (
			"<todo>"
			"<todo>"
			))
	(path :alias repo-url :name repo-url-node) ; todo: (url node)
	(path :alias repo-path :name repo-path-node)
	(end)
)
