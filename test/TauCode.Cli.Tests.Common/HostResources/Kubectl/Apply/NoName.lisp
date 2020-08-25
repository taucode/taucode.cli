(defblock :name dummy-block-name :is-top t
	(executor
		:description "Apply a configuration to a resource by filename or stdin"
		:usage-samples (
			"kubectl apply -f ./pod.json"
			"kubectl apply -k dir/"
			)
	)

	(alt
		(seq
			(multi-text
				:classes key
				:values "-f" "--filename"
				:alias filename
				:action key
				:is-single t)
			(some-text
				:classes path
				:action value
				:description "File to apply configuration to a pod"
				:doc-subst "file")
		)
		(seq
			(multi-text
				:classes key
				:values "-k" "--kustomize"
				:alias kustomize
				:action key
				:is-single t)
			(some-text
				:classes path
				:action value
				:description "Directory to apply resources from"
				:doc-subst "directory")
		)
	)

	(end)
)
